import React from "react";

interface BadgeProps {
  children: React.ReactNode;
  color?: "green" | "blue" | "red" | "yellow" | "gray";
}

const colorClasses: Record<string, string> = {
  green: "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400",
  blue: "bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-400",
  red: "bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400",
  yellow:
    "bg-yellow-100 text-yellow-700 dark:bg-yellow-900/30 dark:text-yellow-400",
  gray: "bg-brand-200 text-brand-700 dark:bg-brand-800 dark:text-brand-300",
};

const Badge: React.FC<BadgeProps> = ({ children, color = "gray" }) => (
  <span
    className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium ${colorClasses[color]}`}
  >
    {children}
  </span>
);

export default Badge;
