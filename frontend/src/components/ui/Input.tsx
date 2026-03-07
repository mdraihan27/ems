import React, { forwardRef } from "react";

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
}

const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, className = "", id, ...rest }, ref) => {
    const inputId = id || label?.toLowerCase().replace(/\s+/g, "-");
    return (
      <div className="w-full">
        {label && (
          <label
            htmlFor={inputId}
            className="block text-sm font-medium text-brand-700 dark:text-brand-300 mb-1"
          >
            {label}
          </label>
        )}
        <input
          ref={ref}
          id={inputId}
          className={`w-full rounded-lg border border-brand-300 bg-white px-3 py-2 text-sm text-brand-900 placeholder-brand-400 transition-colors focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-400/30 dark:border-brand-700 dark:bg-brand-900 dark:text-brand-100 dark:placeholder-brand-600 dark:focus:border-brand-500 ${
            error
              ? "border-red-400 focus:border-red-500 focus:ring-red-400/30"
              : ""
          } ${className}`}
          {...rest}
        />
        {error && (
          <p className="mt-1 text-xs text-red-500 dark:text-red-400">
            {error}
          </p>
        )}
      </div>
    );
  }
);

Input.displayName = "Input";
export default Input;
