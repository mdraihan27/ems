import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  employeeService,
  spouseService,
  childService,
} from "services/employeeService";
import {
  EmployeeQueryParams,
  CreateEmployeeRequest,
  UpdateEmployeeRequest,
  CreateSpouseRequest,
  UpdateSpouseRequest,
  CreateChildRequest,
  UpdateChildRequest,
} from "types";
import toast from "react-hot-toast";

/* Query keys */
const keys = {
  all: ["employees"] as const,
  lists: () => [...keys.all, "list"] as const,
  list: (params: EmployeeQueryParams) => [...keys.lists(), params] as const,
  detail: (id: string) => [...keys.all, "detail", id] as const,
  search: (params: EmployeeQueryParams) =>
    [...keys.all, "search", params] as const,
};

/* List / search */
export function useEmployees(params: EmployeeQueryParams = {}) {
  return useQuery({
    queryKey: keys.list(params),
    queryFn: () => employeeService.list(params).then((r) => r.data.data),
  });
}

export function useEmployeeSearch(params: EmployeeQueryParams) {
  return useQuery({
    queryKey: keys.search(params),
    queryFn: () => employeeService.search(params).then((r) => r.data.data),
    enabled: !!params.q,
  });
}

export function useEmployee(id: string) {
  return useQuery({
    queryKey: keys.detail(id),
    queryFn: () => employeeService.get(id).then((r) => r.data.data),
    enabled: !!id,
  });
}

/* Mutations */
export function useCreateEmployee() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateEmployeeRequest) =>
      employeeService.create(data).then((r) => r.data.data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: keys.all });
      toast.success("Employee created successfully");
    },
  });
}

export function useUpdateEmployee(id: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: UpdateEmployeeRequest) =>
      employeeService.update(id, data).then((r) => r.data.data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: keys.all });
      toast.success("Employee updated successfully");
    },
  });
}

export function useDeleteEmployee() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => employeeService.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: keys.all });
      toast.success("Employee deleted successfully");
    },
  });
}

/* Spouse mutations */
export function useAddSpouse(employeeId: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateSpouseRequest) =>
      spouseService.add(employeeId, data).then((r) => r.data.data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: keys.detail(employeeId) });
      toast.success("Spouse added successfully");
    },
  });
}

export function useUpdateSpouse(employeeId: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: UpdateSpouseRequest) =>
      spouseService.update(employeeId, data).then((r) => r.data.data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: keys.detail(employeeId) });
      toast.success("Spouse updated successfully");
    },
  });
}

export function useDeleteSpouse(employeeId: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: () => spouseService.delete(employeeId),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: keys.detail(employeeId) });
      toast.success("Spouse deleted successfully");
    },
  });
}

/* Child mutations */
export function useAddChild(employeeId: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateChildRequest) =>
      childService.add(employeeId, data).then((r) => r.data.data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: keys.detail(employeeId) });
      toast.success("Child added successfully");
    },
  });
}

export function useUpdateChild(employeeId: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ childId, data }: { childId: string; data: UpdateChildRequest }) =>
      childService.update(employeeId, childId, data).then((r) => r.data.data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: keys.detail(employeeId) });
      toast.success("Child updated successfully");
    },
  });
}

export function useDeleteChild(employeeId: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (childId: string) =>
      childService.delete(employeeId, childId),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: keys.detail(employeeId) });
      toast.success("Child deleted successfully");
    },
  });
}
