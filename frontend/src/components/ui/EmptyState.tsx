import React from "react";

interface EmptyStateProps {
  title?: string;
  description?: string;
  icon?: React.ReactNode;
  action?: React.ReactNode;
}

const EmptyState: React.FC<EmptyStateProps> = ({
  title = "No data found",
  description = "There are no records to display.",
  icon,
  action,
}) => (
  <div className="flex flex-col items-center justify-center py-16 text-center">
    {icon || (
      <div className="rounded-full bg-brand-100 p-4 dark:bg-brand-800 mb-4">
        <svg
          className="h-8 w-8 text-brand-500"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"
          />
        </svg>
      </div>
    )}
    <h3 className="text-lg font-medium text-brand-800 dark:text-brand-200 mb-1">
      {title}
    </h3>
    <p className="text-sm text-brand-500 dark:text-brand-400 mb-4 max-w-sm">
      {description}
    </p>
    {action}
  </div>
);

export default EmptyState;
