import React, { useState, useRef } from "react";
import { motion } from "framer-motion";
import {
  HiDocumentDownload,
  HiSearch,
  HiSortAscending,
  HiSortDescending,
  HiX,
  HiUser,
  HiIdentification,
} from "react-icons/hi";
import Button from "components/ui/Button";
import Card from "components/ui/Card";
import { useEmployees } from "hooks/useEmployees";
import { useDebounce } from "hooks/useDebounce";
import { Employee } from "types";
import { reportService } from "services/employeeService";
import { saveAs } from "file-saver";
import toast from "react-hot-toast";

const GUID_RE =
  /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;

const ReportsPage: React.FC = () => {
  const [listFilter, setListFilter] = useState("");
  const [listSortBy, setListSortBy] = useState("");
  const [listSortDir, setListSortDir] = useState<"asc" | "desc">("asc");
  const [downloadingList, setDownloadingList] = useState(false);

  /* Individual CV download */
  const [cvSearch, setCvSearch] = useState("");
  const [selectedEmployee, setSelectedEmployee] =
    useState<Pick<Employee, "id" | "name" | "department"> | null>(null);
  const [showDropdown, setShowDropdown] = useState(false);
  const [downloadingCv, setDownloadingCv] = useState(false);
  const cvInputRef = useRef<HTMLInputElement>(null);

  const debouncedCvSearch = useDebounce(cvSearch, 300);
  const isGuid = GUID_RE.test(debouncedCvSearch.trim());

  const { data: searchData, isFetching: isSearching } = useEmployees({
    q:
      !isGuid &&
      debouncedCvSearch.trim().length >= 2 &&
      !selectedEmployee
        ? debouncedCvSearch.trim()
        : undefined,
    pageSize: 6,
  });
  const suggestions = searchData?.items ?? [];
  const showSuggestions =
    showDropdown &&
    !isGuid &&
    !selectedEmployee &&
    debouncedCvSearch.trim().length >= 2;

  const handleCvSelect = (
    emp: Pick<Employee, "id" | "name" | "department">
  ) => {
    setSelectedEmployee(emp);
    setCvSearch(emp.name);
    setShowDropdown(false);
  };

  const handleCvClear = () => {
    setSelectedEmployee(null);
    setCvSearch("");
    setShowDropdown(false);
    setTimeout(() => cvInputRef.current?.focus(), 0);
  };

  const handleDownloadList = async () => {
    setDownloadingList(true);
    try {
      const res = await reportService.downloadEmployeeList({
        q: listFilter || undefined,
        sortBy: listSortBy || undefined,
        sortDirection: listSortDir,
      });
      saveAs(res.data, "EmployeeReport.pdf");
      toast.success("Report downloaded");
    } catch {
      toast.error("Failed to download report");
    } finally {
      setDownloadingList(false);
    }
  };

  const handleDownloadCv = async () => {
    const targetId = isGuid ? debouncedCvSearch.trim() : selectedEmployee?.id;
    if (!targetId) {
      toast.error("Search and select an employee, or paste a valid GUID");
      return;
    }
    setDownloadingCv(true);
    try {
      const res = await reportService.downloadEmployeeCv(targetId);
      const cvFilename = selectedEmployee
        ? `${selectedEmployee.name}_${selectedEmployee.department}_${selectedEmployee.id}`
            .replace(/\s+/g, "_")
            .replace(/[^\w-]/g, "")
        : `cv_${targetId}`;
      saveAs(res.data, `${cvFilename}.pdf`);
      toast.success("CV downloaded");
    } catch {
      toast.error("Failed to download CV");
    } finally {
      setDownloadingCv(false);
    }
  };

  const sortOptions = [
    { value: "Name", label: "Name" },
    { value: "Department", label: "Dept" },
    { value: "BasicSalary", label: "Salary" },
  ];

  return (
    <motion.div
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ duration: 0.3 }}
      className="space-y-6"
    >
      <div>
        <h1 className="text-2xl font-bold text-brand-900 dark:text-brand-100">
          Reports
        </h1>
        <p className="text-sm text-brand-500 dark:text-brand-400">
          Generate and download PDF reports
        </p>
      </div>

      {/* Employee List Report */}
      <Card>
        <h3 className="text-base font-semibold text-brand-800 dark:text-brand-200 mb-4">
          Employee List Report
        </h3>
        <p className="text-sm text-brand-500 dark:text-brand-400 mb-4">
          Download a formatted PDF table of all employees with optional filters.
        </p>

        <div className="space-y-3">
          <div className="relative">
            <HiSearch className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-brand-400" />
            <input
              type="text"
              value={listFilter}
              onChange={(e) => setListFilter(e.target.value)}
              placeholder="Filter by name, NID, or department..."
              className="w-full rounded-lg border border-brand-300 bg-white pl-9 pr-3 py-2 text-sm text-brand-900 placeholder-brand-400 focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-400/30 dark:border-brand-700 dark:bg-brand-900 dark:text-brand-100 dark:placeholder-brand-600"
            />
          </div>

          <div className="flex flex-wrap items-center gap-2">
            <span className="text-xs text-brand-500 dark:text-brand-400">
              Sort:
            </span>
            {sortOptions.map((opt) => (
              <button
                key={opt.value}
                onClick={() => {
                  if (listSortBy === opt.value) {
                    setListSortDir((d) => (d === "asc" ? "desc" : "asc"));
                  } else {
                    setListSortBy(opt.value);
                    setListSortDir("asc");
                  }
                }}
                className={`inline-flex items-center gap-1 rounded-lg px-2.5 py-1.5 text-xs font-medium transition-colors ${
                  listSortBy === opt.value
                    ? "bg-brand-600 text-white dark:bg-brand-500"
                    : "bg-brand-100 text-brand-700 hover:bg-brand-200 dark:bg-brand-800 dark:text-brand-300"
                }`}
              >
                {opt.label}
                {listSortBy === opt.value &&
                  (listSortDir === "asc" ? (
                    <HiSortAscending className="h-3.5 w-3.5" />
                  ) : (
                    <HiSortDescending className="h-3.5 w-3.5" />
                  ))}
              </button>
            ))}
          </div>

          <Button
            onClick={handleDownloadList}
            loading={downloadingList}
            icon={<HiDocumentDownload className="h-4 w-4" />}
          >
            Download Employee List PDF
          </Button>
        </div>
      </Card>

      {/* Individual Employee CV */}
      <Card>
        <h3 className="text-base font-semibold text-brand-800 dark:text-brand-200 mb-1">
          Employee CV Report
        </h3>
        <p className="text-sm text-brand-500 dark:text-brand-400 mb-4">
          Search by name, NID, or department — or paste a GUID directly.
        </p>

        <div className="space-y-3">
          {/* Search input */}
          <div className="relative">
            <HiSearch className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-brand-400 pointer-events-none" />
            <input
              ref={cvInputRef}
              type="text"
              value={cvSearch}
              onChange={(e) => {
                setCvSearch(e.target.value);
                setSelectedEmployee(null);
                setShowDropdown(true);
              }}
              onFocus={() => setShowDropdown(true)}
              onBlur={() => setTimeout(() => setShowDropdown(false), 150)}
              placeholder="Search by name / NID / dept, or paste GUID..."
              className="w-full rounded-lg border border-brand-300 bg-white pl-9 pr-8 py-2 text-sm text-brand-900 placeholder-brand-400 focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-400/30 dark:border-brand-700 dark:bg-brand-900 dark:text-brand-100 dark:placeholder-brand-600"
            />
            {cvSearch && (
              <button
                onClick={handleCvClear}
                className="absolute right-2.5 top-1/2 -translate-y-1/2 text-brand-400 hover:text-brand-600 dark:hover:text-brand-300"
                tabIndex={-1}
              >
                <HiX className="h-4 w-4" />
              </button>
            )}

            {/* Suggestions dropdown */}
            {showSuggestions && (
              <div className="absolute z-20 mt-1 w-full rounded-lg border border-brand-200 bg-white shadow-lg dark:border-brand-700 dark:bg-brand-900">
                {isSearching ? (
                  <p className="px-4 py-3 text-sm text-brand-400">Searching...</p>
                ) : suggestions.length === 0 ? (
                  <p className="px-4 py-3 text-sm text-brand-400">No employees found</p>
                ) : (
                  <ul>
                    {suggestions.map((emp) => (
                      <li key={emp.id}>
                        <button
                          onMouseDown={(e) => e.preventDefault()}
                          onClick={() => handleCvSelect(emp)}
                          className="flex w-full items-center gap-3 px-4 py-2.5 text-left text-sm hover:bg-brand-50 dark:hover:bg-brand-800 transition-colors first:rounded-t-lg last:rounded-b-lg"
                        >
                          <div className="flex h-7 w-7 items-center justify-center rounded-full bg-brand-100 dark:bg-brand-700 flex-shrink-0">
                            <HiUser className="h-4 w-4 text-brand-500" />
                          </div>
                          <div className="min-w-0">
                            <p className="font-medium text-brand-900 dark:text-brand-100 truncate">
                              {emp.name}
                            </p>
                            <p className="text-xs text-brand-500 dark:text-brand-400 truncate">
                              {emp.department} &middot; {emp.nid}
                            </p>
                          </div>
                        </button>
                      </li>
                    ))}
                  </ul>
                )}
              </div>
            )}
          </div>

          {/* Selection / GUID indicator */}
          {selectedEmployee && (
            <div className="flex items-center gap-2 rounded-lg bg-brand-50 dark:bg-brand-800/50 px-3 py-2 text-sm">
              <HiUser className="h-4 w-4 text-brand-500 flex-shrink-0" />
              <span className="flex-1 truncate text-brand-700 dark:text-brand-300">
                <span className="font-medium">{selectedEmployee.name}</span>
                <span className="text-brand-400"> &middot; {selectedEmployee.department}</span>
              </span>
            </div>
          )}
          {isGuid && cvSearch.trim() && !selectedEmployee && (
            <div className="flex items-center gap-2 rounded-lg bg-brand-50 dark:bg-brand-800/50 px-3 py-2 text-sm">
              <HiIdentification className="h-4 w-4 text-brand-500 flex-shrink-0" />
              <span className="text-brand-500 dark:text-brand-400 flex-shrink-0">GUID:</span>
              <code className="font-mono text-xs text-brand-700 dark:text-brand-300 truncate">
                {cvSearch.trim()}
              </code>
            </div>
          )}

          <Button
            onClick={handleDownloadCv}
            loading={downloadingCv}
            disabled={!selectedEmployee && !isGuid}
            icon={<HiDocumentDownload className="h-4 w-4" />}
          >
            Download CV PDF
          </Button>
        </div>
      </Card>
    </motion.div>
  );
};

export default ReportsPage;
