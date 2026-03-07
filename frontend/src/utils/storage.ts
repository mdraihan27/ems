import { AuthData } from "types";

const TOKEN_KEY = "ems_auth";

export function getStoredAuth(): AuthData | null {
  try {
    const raw = localStorage.getItem(TOKEN_KEY);
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
}

export function setStoredAuth(data: AuthData): void {
  localStorage.setItem(TOKEN_KEY, JSON.stringify(data));
}

export function clearStoredAuth(): void {
  localStorage.removeItem(TOKEN_KEY);
}

export function isTokenExpired(expiresAt: string): boolean {
  return new Date(expiresAt) <= new Date();
}
