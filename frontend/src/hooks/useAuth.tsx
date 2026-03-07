import React, {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";
import { AuthData, UserRole } from "types";
import {
  getStoredAuth,
  setStoredAuth,
  clearStoredAuth,
} from "utils/storage";
import { authService } from "services/authService";

interface AuthContextValue {
  auth: AuthData | null;
  isAuthenticated: boolean;
  isAdmin: boolean;
  userRole: UserRole | null;
  login: (email: string, password: string) => Promise<void>;
  register: (
    email: string,
    password: string,
    fullName: string,
    role: UserRole
  ) => Promise<void>;
  logout: () => void;
  loading: boolean;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [auth, setAuth] = useState<AuthData | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const stored = getStoredAuth();
    if (stored) setAuth(stored);
    setLoading(false);
  }, []);

  const login = useCallback(async (email: string, password: string) => {
    const res = await authService.login({ email, password });
    const data = res.data.data;
    setStoredAuth(data);
    setAuth(data);
  }, []);

  const register = useCallback(
    async (
      email: string,
      password: string,
      fullName: string,
      role: UserRole
    ) => {
      const res = await authService.register({
        email,
        password,
        fullName,
        role,
      });
      const data = res.data.data;
      setStoredAuth(data);
      setAuth(data);
    },
    []
  );

  const logout = useCallback(() => {
    if (auth?.refreshToken) {
      authService.revoke(auth.refreshToken).catch(() => {});
    }
    clearStoredAuth();
    setAuth(null);
  }, [auth?.refreshToken]);

  const value = useMemo<AuthContextValue>(() => {
    const roles = auth?.roles ?? [];
    return {
      auth,
      isAuthenticated: !!auth,
      isAdmin: roles.includes("Admin"),
      userRole: roles.includes("Admin")
        ? "Admin"
        : roles.includes("Viewer")
        ? "Viewer"
        : null,
      login,
      register,
      logout,
      loading,
    };
  }, [auth, login, register, logout, loading]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
