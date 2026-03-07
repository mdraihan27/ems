import React, { useState, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";
import {
  HiSearch,
  HiPlus,
  HiSortAscending,
  HiSortDescending,
  HiEye,
  HiPencil,
  HiTrash,
  HiHeart,
  HiUserGroup,
} from "react-icons/hi";
import Input from "components/ui/Input";
import Button from "components/ui/Button";
import Badge from "components/ui/Badge";
import Card from "components/ui/Card";
import ErrorState from "components/ui/ErrorState";
import EmptyState from "components/ui/EmptyState";
import { TableSkeleton } from "components/ui/Skeleton";
import Pagination from "components/common/Pagination";
import ConfirmDialog from "components/common/ConfirmDialog";
import { useEmployees, useDeleteEmployee } from "hooks/useEmployees";
import { useAuth } from "hooks/useAuth";
import { useDebounce } from "hooks/useDebounce";
import { formatCurrency, formatPhone } from "utils/helpers";
import { EmployeeQueryParams } from "types";
import toast from "react-hot-toast";
import { AxiosError } from "axios";
import { ApiResponse } from "types";

const sortOptions = [
  { value: "Name", label: "Name" },
  { value: "Department", label: "Department" },
  { value: "NID", label: "NID" },
  { value: "BasicSalary", label: "Salary" },
];

const EmployeeListPage: React.FC = () => {
  const navigate = useNavigate();
  const { isAdmin } = useAuth();

  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const [sortBy, setSortBy] = useState<string>("");
  const [sortDir, setSortDir] = useState<"asc" | "desc">("asc");
  const [deleteId, setDeleteId] = useState<string | null>(null);

  const debouncedSearch = useDebounce(search, 400);

  const params = useMemo<EmployeeQueryParams>(
    () => ({
      page,
      pageSize: 10,
      sortBy: sortBy || undefined,
      sortDirection: sortDir,
      q: debouncedSearch || undefined,
    }),
    [page, sortBy, sortDir, debouncedSearch]
  );

  const { data, isLoading, isError, refetch } = useEmployees(params);
  const deleteMutation = useDeleteEmployee();

  const toggleSort = (field: string) => {
    if (sortBy === field) {
      setSortDir((d) => (d === "asc" ? "desc" : "asc"));
    } else {
      setSortBy(field);
      setSortDir("asc");
    }
    setPage(1);
  };

  const handleDelete = async () => {
    if (!deleteId) return;
    try {
      await deleteMutation.mutateAsync(deleteId);
      setDeleteId(null);
    } catch (err) {
      const axiosErr = err as AxiosError<ApiResponse<null>>;
      toast.error(axiosErr.response?.data?.message || "Failed to delete");
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ duration: 0.3 }}
      className="space-y-6"
    >
      {/* Page header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-brand-900 dark:text-brand-100">
            Employees
          </h1>
          <p className="text-sm text-brand-500 dark:text-brand-400">
            Manage employee records
          </p>
        </div>
        {isAdmin && (
          <Button
            onClick={() => navigate("/employees/new")}
            icon={<HiPlus className="h-4 w-4" />}
          >
            Add Employee
          </Button>
        )}
      </div>

      {/* Filters */}
      <Card className="!p-4">
        <div className="flex flex-col sm:flex-row gap-3">
          <div className="flex-1 relative">
            <HiSearch className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-brand-400" />
            <input
              type="text"
              value={search}
              onChange={(e) => {
                setSearch(e.target.value);
                setPage(1);
              }}
              placeholder="Search by name, NID, department, or salary..."
              className="w-full rounded-lg border border-brand-300 bg-white pl-9 pr-3 py-2 text-sm text-brand-900 placeholder-brand-400 transition focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-400/30 dark:border-brand-700 dark:bg-brand-900 dark:text-brand-100 dark:placeholder-brand-600"
            />
          </div>
          <div className="flex gap-2 flex-wrap">
            {sortOptions.map((opt) => (
              <button
                key={opt.value}
                onClick={() => toggleSort(opt.value)}
                className={`inline-flex items-center gap-1 rounded-lg px-3 py-2 text-xs font-medium transition-colors ${
                  sortBy === opt.value
                    ? "bg-brand-600 text-white dark:bg-brand-500"
                    : "bg-brand-100 text-brand-700 hover:bg-brand-200 dark:bg-brand-800 dark:text-brand-300 dark:hover:bg-brand-700"
                }`}
              >
                {opt.label}
                {sortBy === opt.value &&
                  (sortDir === "asc" ? (
                    <HiSortAscending className="h-3.5 w-3.5" />
                  ) : (
                    <HiSortDescending className="h-3.5 w-3.5" />
                  ))}
              </button>
            ))}
          </div>
        </div>
      </Card>

      {/* Table */}
      {isLoading ? (
        <Card>
          <TableSkeleton rows={8} cols={7} />
        </Card>
      ) : isError ? (
        <ErrorState onRetry={refetch} />
      ) : !data || data.items.length === 0 ? (
        <EmptyState
          title="No employees found"
          description={
            debouncedSearch
              ? `No results for "${debouncedSearch}"`
              : "Get started by adding your first employee."
          }
          action={
            isAdmin ? (
              <Button
                onClick={() => navigate("/employees/new")}
                icon={<HiPlus className="h-4 w-4" />}
              >
                Add Employee
              </Button>
            ) : undefined
          }
        />
      ) : (
        <Card className="!p-0 overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-brand-200 dark:border-brand-700 bg-brand-50/50 dark:bg-brand-900/50">
                  <th className="py-3 px-4 text-left font-medium text-brand-500 dark:text-brand-400">
                    Name
                  </th>
                  <th className="py-3 px-4 text-left font-medium text-brand-500 dark:text-brand-400 hidden md:table-cell">
                    NID
                  </th>
                  <th className="py-3 px-4 text-left font-medium text-brand-500 dark:text-brand-400 hidden sm:table-cell">
                    Phone
                  </th>
                  <th className="py-3 px-4 text-left font-medium text-brand-500 dark:text-brand-400">
                    Department
                  </th>
                  <th className="py-3 px-4 text-right font-medium text-brand-500 dark:text-brand-400 hidden sm:table-cell">
                    Salary
                  </th>
                  <th className="py-3 px-4 text-center font-medium text-brand-500 dark:text-brand-400 hidden lg:table-cell">
                    Family
                  </th>
                  <th className="py-3 px-4 text-right font-medium text-brand-500 dark:text-brand-400">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody>
                {data.items.map((emp, idx) => (
                  <motion.tr
                    key={emp.id}
                    initial={{ opacity: 0, x: -8 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{ delay: idx * 0.03 }}
                    className="border-b border-brand-100 dark:border-brand-800 hover:bg-brand-50/50 dark:hover:bg-brand-800/30 transition-colors"
                  >
                    <td className="py-3 px-4">
                      <button
                        onClick={() => navigate(`/employees/${emp.id}`)}
                        className="font-medium text-brand-900 dark:text-brand-100 hover:text-brand-600 dark:hover:text-brand-400 hover:underline text-left transition-colors"
                      >
                        {emp.name}
                      </button>
                    </td>
                    <td className="py-3 px-4 text-brand-600 dark:text-brand-400 hidden md:table-cell font-mono text-xs">
                      {emp.nid}
                    </td>
                    <td className="py-3 px-4 text-brand-600 dark:text-brand-400 hidden sm:table-cell text-xs">
                      {formatPhone(emp.phone)}
                    </td>
                    <td className="py-3 px-4">
                      <Badge color="green">{emp.department}</Badge>
                    </td>
                    <td className="py-3 px-4 text-right text-brand-700 dark:text-brand-300 hidden sm:table-cell">
                      {formatCurrency(emp.basicSalary)}
                    </td>
                    <td className="py-3 px-4 hidden lg:table-cell">
                      <div className="flex items-center justify-center gap-2">
                        {emp.spouse && (
                          <span title="Has spouse">
                            <HiHeart className="h-4 w-4 text-pink-400" />
                          </span>
                        )}
                        {emp.children.length > 0 && (
                          <span className="flex items-center gap-0.5 text-xs text-brand-500 dark:text-brand-400" title="Children">
                            <HiUserGroup className="h-4 w-4" />
                            {emp.children.length}
                          </span>
                        )}
                      </div>
                    </td>
                    <td className="py-3 px-4">
                      <div className="flex items-center justify-end gap-1">
                        <button
                          onClick={() => navigate(`/employees/${emp.id}`)}
                          className="rounded-lg p-1.5 text-brand-500 hover:bg-brand-100 dark:hover:bg-brand-800 transition-colors"
                          title="View"
                        >
                          <HiEye className="h-4 w-4" />
                        </button>
                        {isAdmin && (
                          <>
                            <button
                              onClick={() =>
                                navigate(`/employees/${emp.id}/edit`)
                              }
                              className="rounded-lg p-1.5 text-brand-500 hover:bg-brand-100 dark:hover:bg-brand-800 transition-colors"
                              title="Edit"
                            >
                              <HiPencil className="h-4 w-4" />
                            </button>
                            <button
                              onClick={() => setDeleteId(emp.id)}
                              className="rounded-lg p-1.5 text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors"
                              title="Delete"
                            >
                              <HiTrash className="h-4 w-4" />
                            </button>
                          </>
                        )}
                      </div>
                    </td>
                  </motion.tr>
                ))}
              </tbody>
            </table>
          </div>

          {data.pagination && (
            <div className="px-4 pb-4">
              <Pagination
                pagination={data.pagination}
                onPageChange={setPage}
              />
            </div>
          )}
        </Card>
      )}

      {/* Delete confirmation */}
      <ConfirmDialog
        open={!!deleteId}
        onClose={() => setDeleteId(null)}
        onConfirm={handleDelete}
        title="Delete Employee"
        message="This will permanently delete this employee and all their family data. This action cannot be undone."
        loading={deleteMutation.isPending}
      />
    </motion.div>
  );
};

export default EmployeeListPage;
