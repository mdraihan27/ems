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
  email: z.string().min(1, "Email is required").email("Invalid email"),
  password: z.string().min(1, "Password is required"),
});

type FormValues = z.infer<typeof schema>;

const LoginPage: React.FC = () => {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormValues>({ resolver: zodResolver(schema) });

  const onSubmit = async (values: FormValues) => {
    setSubmitting(true);
    try {
      await login(values.email, values.password);
      toast.success("Welcome back!");
      navigate("/dashboard");
    } catch (err) {
      const axiosErr = err as AxiosError<ApiResponse<null>>;
      const serverErrors = extractApiErrors(
        axiosErr.response?.data?.errors ?? null
      );
      const message =
        serverErrors.length > 0
          ? serverErrors.join(", ")
          : axiosErr.response?.data?.message || "Login failed";
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
        <h2 className="text-xl font-semibold text-brand-900 dark:text-brand-100 mb-6">
          Sign in to your account
        </h2>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <Input
            label="Email"
            type="email"
            placeholder="admin@example.com"
            error={errors.email?.message}
            autoComplete="email"
            {...register("email")}
          />

          <Input
            label="Password"
            type="password"
            placeholder="••••••••"
            error={errors.password?.message}
            autoComplete="current-password"
            {...register("password")}
          />

          <Button
            type="submit"
            className="w-full"
            size="lg"
            loading={submitting}
          >
            Sign In
          </Button>
        </form>

        <p className="mt-6 text-center text-sm text-brand-500 dark:text-brand-400">
          Don't have an account?{" "}
          <Link
            to="/register"
            className="font-medium text-brand-700 hover:text-brand-800 dark:text-brand-300 dark:hover:text-brand-200 transition-colors"
          >
            Register
          </Link>
        </p>
      </div>
    </motion.div>
  );
};

export default LoginPage;
