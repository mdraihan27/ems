import React from "react";

interface SkeletonProps {
  className?: string;
  count?: number;
}

const Skeleton: React.FC<SkeletonProps> = ({ className = "", count = 1 }) => {
  return (
    <>
      {Array.from({ length: count }).map((_, i) => (
        <div
          key={i}
          className={`animate-pulse rounded-md bg-brand-200 dark:bg-brand-800 ${className}`}
        />
      ))}
    </>
  );
};

export const TableSkeleton: React.FC<{ rows?: number; cols?: number }> = ({
  rows = 5,
  cols = 6,
}) => (
  <div className="space-y-3">
    <div className="grid gap-4" style={{ gridTemplateColumns: `repeat(${cols}, 1fr)` }}>
      {Array.from({ length: cols }).map((_, i) => (
        <Skeleton key={i} className="h-8" />
      ))}
    </div>
    {Array.from({ length: rows }).map((_, row) => (
      <div
        key={row}
        className="grid gap-4"
        style={{ gridTemplateColumns: `repeat(${cols}, 1fr)` }}
      >
        {Array.from({ length: cols }).map((_, col) => (
          <Skeleton key={col} className="h-6" />
        ))}
      </div>
    ))}
  </div>
);

export const CardSkeleton: React.FC = () => (
  <div className="rounded-xl border border-brand-200 bg-white p-6 dark:border-brand-800 dark:bg-brand-900">
    <Skeleton className="h-4 w-1/3 mb-4" />
    <Skeleton className="h-8 w-1/2 mb-2" />
    <Skeleton className="h-4 w-2/3" />
  </div>
);

export default Skeleton;
