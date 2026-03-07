import React, { lazy, Suspense } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import MainLayout from "components/layouts/MainLayout";
import AuthLayout from "components/layouts/AuthLayout";
import ProtectedRoute from "./ProtectedRoute";
import { PageLoader } from "components/ui/Spinner";

/* Lazy-loaded pages */
const LandingPage = lazy(() => import("pages/LandingPage"));
const LoginPage = lazy(() => import("pages/LoginPage"));
const RegisterPage = lazy(() => import("pages/RegisterPage"));
const DashboardPage = lazy(() => import("pages/DashboardPage"));
const EmployeeListPage = lazy(() => import("pages/EmployeeListPage"));
const EmployeeDetailPage = lazy(() => import("pages/EmployeeDetailPage"));
const EmployeeFormPage = lazy(() => import("pages/EmployeeFormPage"));
const ReportsPage = lazy(() => import("pages/ReportsPage"));

const AppRoutes: React.FC = () => {
  return (
    <Suspense fallback={<PageLoader />}>
      <Routes>
        {/* Landing page */}
        <Route path="/" element={<LandingPage />} />

        {/* Auth routes */}
        <Route element={<AuthLayout />}>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Route>

        {/* Protected routes */}
        <Route
          element={
            <ProtectedRoute>
              <MainLayout />
            </ProtectedRoute>
          }
        >
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/employees" element={<EmployeeListPage />} />
          <Route path="/employees/:id" element={<EmployeeDetailPage />} />
          <Route
            path="/employees/new"
            element={
              <ProtectedRoute requiredRole="Admin">
                <EmployeeFormPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/employees/:id/edit"
            element={
              <ProtectedRoute requiredRole="Admin">
                <EmployeeFormPage />
              </ProtectedRoute>
            }
          />
          <Route path="/reports" element={<ReportsPage />} />
        </Route>

        {/* Catch-all */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Suspense>
  );
};

export default AppRoutes;
