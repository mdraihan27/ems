import React from "react";
import Button from "./Button";
import { HiRefresh } from "react-icons/hi";

interface ErrorStateProps {
  message?: string;
  onRetry?: () => void;
}

const ErrorState: React.FC<ErrorStateProps> = ({
  message = "Something went wrong. Please try again.",
  onRetry,
}) => (
  <div className="flex flex-col items-center justify-center py-16 text-center">
    <div className="rounded-full bg-red-100 p-4 dark:bg-red-900/30 mb-4">
      <svg
        className="h-8 w-8 text-red-500"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth={2}
          d="M12 9v2m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
        />
      </svg>
    </div>
    <p className="text-brand-600 dark:text-brand-400 mb-4 max-w-md">
      {message}
    </p>
    {onRetry && (
      <Button variant="secondary" onClick={onRetry} icon={<HiRefresh className="h-4 w-4" />}>
        Try Again
      </Button>
    )}
  </div>
);

export default ErrorState;
