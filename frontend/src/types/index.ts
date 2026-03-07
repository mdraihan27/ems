/* API response types */
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: Record<string, string[]> | null;
}

export interface PaginatedData<T> {
  items: T[];
  pagination: Pagination;
}

export interface Pagination {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

/* Auth types */
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  role: "Admin" | "Viewer";
}

export interface AuthData {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  email: string;
  roles: string[];
}

export interface RefreshRequest {
  accessToken: string;
  refreshToken: string;
}

/* Employee types */
export interface Employee {
  id: string;
  name: string;
  nid: string;
  phone: string;
  department: string;
  basicSalary: number;
  rowVersion: number;
  spouse: Spouse | null;
  children: Child[];
}

export interface CreateEmployeeRequest {
  name: string;
  nid: string;
  phone: string;
  department: string;
  basicSalary: number;
}

export interface UpdateEmployeeRequest extends CreateEmployeeRequest {
  rowVersion: number;
}

/* Spouse types */
export interface Spouse {
  id: string;
  name: string;
  nid: string;
  rowVersion: number;
}

export interface CreateSpouseRequest {
  name: string;
  nid: string;
}

export interface UpdateSpouseRequest extends CreateSpouseRequest {
  rowVersion: number;
}

/* Child types */
export interface Child {
  id: string;
  name: string;
  dateOfBirth: string;
  rowVersion: number;
}

export interface CreateChildRequest {
  name: string;
  dateOfBirth: string;
}

export interface UpdateChildRequest extends CreateChildRequest {
  rowVersion: number;
}

/* Query params */
export interface EmployeeQueryParams {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDirection?: "asc" | "desc";
  q?: string;
}

export type UserRole = "Admin" | "Viewer";
