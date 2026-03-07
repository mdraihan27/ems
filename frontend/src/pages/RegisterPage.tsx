import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { motion } from "framer-motion";
import { AxiosError } from "axios";
import toast from "react-hot-toast";
import Input from "components/ui/Input";
import Button from "components/ui/Button";
import { useAuth } from "hooks/useAuth";
import { ApiResponse } from "types";
import { extractApiErrors } from "utils/helpers";

const schema = z.object({
  fullName: z.string().min(1, "Full name is required").max(200),
  email: z.string().min(1, "Email is required").email("Invalid email"),
  password: z
    .string()
    .min(8, "Minimum 8 characters")
    .regex(/[A-Z]/, "Must contain an uppercase letter")
    .regex(/[a-z]/, "Must contain a lowercase letter")
    .regex(/[0-9]/, "Must contain a digit")
    .regex(/[^A-Za-z0-9]/, "Must contain a special character"),
  role: z.enum(["Admin", "Viewer"]),
});

type FormValues = z.infer<typeof schema>;

const RegisterPage: React.FC = () => {
  const { register: registerUser } = useAuth();
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: { role: "Viewer" },
  });

  const onSubmit = async (values: FormValues) => {
    setSubmitting(true);
    try {
      await registerUser(
        values.email,
        values.password,
        values.fullName,
        values.role
      );
      toast.success("Registration successful!");
      navigate("/dashboard");
    } catch (err) {
      const axiosErr = err as AxiosError<ApiResponse<null>>;
      const serverErrors = extractApiErrors(
        axiosErr.response?.data?.errors ?? null
      );
      const message =
        serverErrors.length > 0
          ? serverErrors.join(", ")
          : axiosErr.response?.data?.message || "Registration failed";
      toast.error(message);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 10 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.3, delay: 0.1 }}
    >
      <div className="rounded-2xl border border-brand-200 bg-white p-8 shadow-sm dark:border-brand-800 dark:bg-brand-900">
        <h2 className="text-xl font-semibold text-brand-900 dark:text-brand-100 mb-4">
          Create an account
        </h2>

        {/* Email verification notice */}
        <div className="rounded-lg bg-blue-50 dark:bg-blue-900/20 border border-blue-200 dark:border-blue-800 px-4 py-3 mb-5">
          <p className="text-xs text-blue-700 dark:text-blue-300">
            <span className="font-semibold">Note:</span> Email verification is skipped for this
            project. Your account will be auto-verified and immediately active upon registration.
            <span className="block mt-1 text-blue-500 dark:text-blue-400">
              This application was built as part of the Fionetix job interview assessment.
            </span>
          </p>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <Input
            label="Full Name"
            placeholder="Tanvir Ahmed"
            error={errors.fullName?.message}
            {...register("fullName")}
          />

          <Input
            label="Email"
            type="email"
            placeholder="viewer@example.com"
            error={errors.email?.message}
            autoComplete="email"
            {...register("email")}
          />

          <Input
            label="Password"
            type="password"
            placeholder="••••••••"
            error={errors.password?.message}
            autoComplete="new-password"
            {...register("password")}
          />

          <div className="w-full">
            <label className="block text-sm font-medium text-brand-700 dark:text-brand-300 mb-1">
              Role
            </label>
            <select
              {...register("role")}
              className="w-full rounded-lg border border-brand-300 bg-white px-3 py-2 text-sm text-brand-900 transition-colors focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-400/30 dark:border-brand-700 dark:bg-brand-900 dark:text-brand-100"
            >
              <option value="Viewer">Viewer</option>
              <option value="Admin">Admin</option>
            </select>
          </div>

          <Button
            type="submit"
            className="w-full"
            size="lg"
            loading={submitting}
          >
            Create Account
          </Button>
        </form>

        <p className="mt-6 text-center text-sm text-brand-500 dark:text-brand-400">
          Already have an account?{" "}
          <Link
            to="/login"
            className="font-medium text-brand-700 hover:text-brand-800 dark:text-brand-300 dark:hover:text-brand-200 transition-colors"
          >
            Sign in
          </Link>
        </p>
      </div>
    </motion.div>
  );
};

export default RegisterPage;
