import React from "react";
import Button from "components/ui/Button";
import { HiChevronLeft, HiChevronRight } from "react-icons/hi";
import { Pagination as PaginationType } from "types";

interface PaginationProps {
  pagination: PaginationType;
  onPageChange: (page: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({ pagination, onPageChange }) => {
  const { page, totalPages, totalCount, pageSize } = pagination;
  const start = (page - 1) * pageSize + 1;
  const end = Math.min(page * pageSize, totalCount);

  if (totalPages <= 1) return null;

  return (
    <div className="flex flex-col sm:flex-row items-center justify-between gap-3 pt-4">
      <p className="text-sm text-brand-500 dark:text-brand-400">
        Showing {start}–{end} of {totalCount} results
      </p>
      <div className="flex items-center gap-1">
        <Button
          variant="ghost"
          size="sm"
          disabled={page <= 1}
          onClick={() => onPageChange(page - 1)}
          icon={<HiChevronLeft className="h-4 w-4" />}
        >
          Prev
        </Button>

        {Array.from({ length: totalPages }, (_, i) => i + 1)
          .filter(
            (p) =>
              p === 1 ||
              p === totalPages ||
              (p >= page - 1 && p <= page + 1)
          )
          .reduce<(number | "ellipsis")[]>((acc, p, idx, arr) => {
            if (idx > 0 && p - (arr[idx - 1] as number) > 1) acc.push("ellipsis");
            acc.push(p);
            return acc;
          }, [])
          .map((item, idx) =>
            item === "ellipsis" ? (
              <span
                key={`e-${idx}`}
                className="px-2 text-brand-400 dark:text-brand-600"
              >
                …
              </span>
            ) : (
              <button
                key={item}
                onClick={() => onPageChange(item as number)}
                className={`h-8 w-8 rounded-lg text-sm font-medium transition-colors ${
                  item === page
                    ? "bg-brand-600 text-white dark:bg-brand-500"
                    : "text-brand-600 hover:bg-brand-100 dark:text-brand-400 dark:hover:bg-brand-800"
                }`}
              >
                {item}
              </button>
            )
          )}

        <Button
          variant="ghost"
          size="sm"
          disabled={page >= totalPages}
          onClick={() => onPageChange(page + 1)}
          icon={<HiChevronRight className="h-4 w-4" />}
        >
          Next
        </Button>
      </div>
    </div>
  );
};

export default Pagination;
