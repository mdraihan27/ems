import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { motion } from "framer-motion";
import { HiArrowLeft } from "react-icons/hi";
import { AxiosError } from "axios";
import toast from "react-hot-toast";
import Input from "components/ui/Input";
import Button from "components/ui/Button";
import Card from "components/ui/Card";
import { PageLoader } from "components/ui/Spinner";
import ErrorState from "components/ui/ErrorState";
import {
  useEmployee,
  useCreateEmployee,
  useUpdateEmployee,
} from "hooks/useEmployees";
import { ApiResponse } from "types";
import { extractApiErrors } from "utils/helpers";

const schema = z.object({
  name: z.string().min(1, "Name is required").max(200),
  nid: z
    .string()
    .min(1, "NID is required")
    .regex(/^\d{10}$|^\d{17}$/, "NID must be exactly 10 or 17 digits"),
  phone: z
    .string()
    .min(1, "Phone is required")
    .regex(
      /^(\+880|0)1[3-9]\d{8}$/,
      "Must be valid Bangladesh phone (+8801XXXXXXXXX or 01XXXXXXXXX)"
    ),
  department: z.string().min(1, "Department is required").max(100),
  basicSalary: z.number().min(0, "Salary must be >= 0"),
});

type FormValues = z.infer<typeof schema>;

const EmployeeFormPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  const {
    data: employee,
    isLoading,
    isError,
    refetch,
  } = useEmployee(id ?? "");
  const createMutation = useCreateEmployee();
  const updateMutation = useUpdateEmployee(id ?? "");

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      name: "",
      nid: "",
      phone: "",
      department: "",
      basicSalary: 0,
    },
  });

  useEffect(() => {
    if (isEdit && employee) {
      reset({
        name: employee.name,
        nid: employee.nid,
        phone: employee.phone,
        department: employee.department,
        basicSalary: employee.basicSalary,
      });
    }
  }, [isEdit, employee, reset]);

  if (isEdit && isLoading) return <PageLoader />;
  if (isEdit && isError) return <ErrorState onRetry={refetch} />;

  const onSubmit = async (values: FormValues) => {
    setSubmitting(true);
    try {
      if (isEdit && employee) {
        await updateMutation.mutateAsync({
          ...values,
          rowVersion: employee.rowVersion,
        });
        navigate(`/employees/${id}`);
      } else {
        const created = await createMutation.mutateAsync(values);
        navigate(`/employees/${created.id}`);
      }
    } catch (err) {
      const axiosErr = err as AxiosError<ApiResponse<null>>;
      const serverErrors = extractApiErrors(
        axiosErr.response?.data?.errors ?? null
      );
      const message =
        serverErrors.length > 0
          ? serverErrors.join(", ")
          : axiosErr.response?.data?.message || "Operation failed";
      toast.error(message);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 10 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.3 }}
      className="max-w-2xl mx-auto space-y-6"
    >
      <div className="flex items-center gap-3">
        <Button
          variant="ghost"
          size="sm"
          onClick={() => navigate(-1)}
          icon={<HiArrowLeft className="h-4 w-4" />}
        >
          Back
        </Button>
        <h1 className="text-2xl font-bold text-brand-900 dark:text-brand-100">
          {isEdit ? "Edit Employee" : "Add Employee"}
        </h1>
      </div>

      <Card>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
          <Input
            label="Full Name"
            placeholder="Md. Hasan Ali"
            error={errors.name?.message}
            {...register("name")}
          />

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <Input
              label="NID"
              placeholder="1234567890"
              error={errors.nid?.message}
              {...register("nid")}
            />
            <Input
              label="Phone"
              placeholder="01712345678"
              error={errors.phone?.message}
              {...register("phone")}
            />
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <Input
              label="Department"
              placeholder="Engineering"
              error={errors.department?.message}
              {...register("department")}
            />
            <Input
              label="Basic Salary (BDT)"
              type="number"
              placeholder="75000"
              error={errors.basicSalary?.message}
              {...register("basicSalary", { valueAsNumber: true })}
            />
          </div>

          <div className="flex justify-end gap-3 pt-4 border-t border-brand-100 dark:border-brand-800">
            <Button
              variant="ghost"
              type="button"
              onClick={() => navigate(-1)}
            >
              Cancel
            </Button>
            <Button type="submit" loading={submitting}>
              {isEdit ? "Save Changes" : "Create Employee"}
            </Button>
          </div>
        </form>
      </Card>
    </motion.div>
  );
};

export default EmployeeFormPage;
