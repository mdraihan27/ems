import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "hooks/useAuth";
import { PageLoader } from "components/ui/Spinner";

interface ProtectedRouteProps {
  children: React.ReactNode;
  requiredRole?: "Admin";
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  children,
  requiredRole,
}) => {
  const { isAuthenticated, isAdmin, loading } = useAuth();
  const location = useLocation();

  if (loading) return <PageLoader />;

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (requiredRole === "Admin" && !isAdmin) {
    return <Navigate to="/dashboard" replace />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
