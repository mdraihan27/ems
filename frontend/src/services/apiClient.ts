import axios, { AxiosError, InternalAxiosRequestConfig } from "axios";
import { getStoredAuth, setStoredAuth, clearStoredAuth } from "utils/storage";
import { ApiResponse, AuthData } from "types";

const apiClient = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
  headers: { "Content-Type": "application/json" },
});

/* Attach access token to every request */
apiClient.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  const auth = getStoredAuth();
  if (auth?.accessToken && config.headers) {
    config.headers.Authorization = `Bearer ${auth.accessToken}`;
  }
  return config;
});

/* Track if a token refresh is already in-flight */
let refreshPromise: Promise<AuthData> | null = null;

async function refreshTokens(): Promise<AuthData> {
  const auth = getStoredAuth();
  if (!auth) throw new Error("No auth data");

  const res = await axios.post<ApiResponse<AuthData>>(
    `${process.env.REACT_APP_API_BASE_URL}/auth/refresh`,
    {
      accessToken: auth.accessToken,
      refreshToken: auth.refreshToken,
    }
  );
  const newAuth = res.data.data;
  setStoredAuth(newAuth);
  return newAuth;
}

/* Handle 401 → attempt silent refresh → retry original request */
apiClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const original = error.config as InternalAxiosRequestConfig & {
      _retried?: boolean;
    };

    if (error.response?.status === 401 && !original._retried) {
      original._retried = true;

      try {
        if (!refreshPromise) {
          refreshPromise = refreshTokens();
        }
        const newAuth = await refreshPromise;
        refreshPromise = null;

        if (original.headers) {
          original.headers.Authorization = `Bearer ${newAuth.accessToken}`;
        }
        return apiClient(original);
      } catch {
        refreshPromise = null;
        clearStoredAuth();
        window.location.href = "/login";
        return Promise.reject(error);
      }
    }

    return Promise.reject(error);
  }
);

export default apiClient;
