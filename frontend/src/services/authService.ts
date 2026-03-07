import apiClient from "./apiClient";
import {
  ApiResponse,
  AuthData,
  LoginRequest,
  RegisterRequest,
} from "types";

export const authService = {
  login: (data: LoginRequest) =>
    apiClient.post<ApiResponse<AuthData>>("/auth/login", data),

  register: (data: RegisterRequest) =>
    apiClient.post<ApiResponse<AuthData>>("/auth/register", data),

  revoke: (refreshToken: string) =>
    apiClient.post<ApiResponse<null>>("/auth/revoke", { refreshToken }),
};
