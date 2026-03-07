import apiClient from "./apiClient";
import {
  ApiResponse,
  Employee,
  PaginatedData,
  CreateEmployeeRequest,
  UpdateEmployeeRequest,
  EmployeeQueryParams,
  Spouse,
  CreateSpouseRequest,
  UpdateSpouseRequest,
  Child,
  CreateChildRequest,
  UpdateChildRequest,
} from "types";

/* Employee CRUD */
export const employeeService = {
  list: (params?: EmployeeQueryParams) =>
    apiClient.get<ApiResponse<PaginatedData<Employee>>>("/employees", {
      params,
    }),

  get: (id: string) =>
    apiClient.get<ApiResponse<Employee>>(`/employees/${encodeURIComponent(id)}`),

  search: (params: EmployeeQueryParams) =>
    apiClient.get<ApiResponse<PaginatedData<Employee>>>("/employees/search", {
      params,
    }),

  create: (data: CreateEmployeeRequest) =>
    apiClient.post<ApiResponse<Employee>>("/employees", data),

  update: (id: string, data: UpdateEmployeeRequest) =>
    apiClient.put<ApiResponse<Employee>>(`/employees/${encodeURIComponent(id)}`, data),

  delete: (id: string) =>
    apiClient.delete<ApiResponse<null>>(`/employees/${encodeURIComponent(id)}`),
};

/* Spouse */
export const spouseService = {
  add: (employeeId: string, data: CreateSpouseRequest) =>
    apiClient.post<ApiResponse<Spouse>>(
      `/employees/${encodeURIComponent(employeeId)}/spouse`,
      data
    ),

  update: (employeeId: string, data: UpdateSpouseRequest) =>
    apiClient.put<ApiResponse<Spouse>>(
      `/employees/${encodeURIComponent(employeeId)}/spouse`,
      data
    ),

  delete: (employeeId: string) =>
    apiClient.delete<ApiResponse<null>>(
      `/employees/${encodeURIComponent(employeeId)}/spouse`
    ),
};

/* Children */
export const childService = {
  add: (employeeId: string, data: CreateChildRequest) =>
    apiClient.post<ApiResponse<Child>>(
      `/employees/${encodeURIComponent(employeeId)}/children`,
      data
    ),

  update: (employeeId: string, childId: string, data: UpdateChildRequest) =>
    apiClient.put<ApiResponse<Child>>(
      `/employees/${encodeURIComponent(employeeId)}/children/${encodeURIComponent(childId)}`,
      data
    ),

  delete: (employeeId: string, childId: string) =>
    apiClient.delete<ApiResponse<null>>(
      `/employees/${encodeURIComponent(employeeId)}/children/${encodeURIComponent(childId)}`
    ),
};

/* Reports */
export const reportService = {
  downloadEmployeeList: (params?: {
    q?: string;
    sortBy?: string;
    sortDirection?: string;
  }) =>
    apiClient.get("/employees/reports/list", {
      params,
      responseType: "blob",
    }),

  downloadEmployeeCv: (id: string) =>
    apiClient.get(`/employees/${encodeURIComponent(id)}/reports/cv`, {
      responseType: "blob",
    }),
};
