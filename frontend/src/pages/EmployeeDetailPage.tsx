import React, { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { motion } from "framer-motion";
import {
  HiArrowLeft,
  HiPencil,
  HiTrash,
  HiPhone,
  HiIdentification,
  HiOfficeBuilding,
  HiCurrencyBangladeshi,
  HiHeart,
  HiUserAdd,
  HiPlus,
  HiDocumentDownload,
  HiClipboard,
} from "react-icons/hi";
import Button from "components/ui/Button";
import Card from "components/ui/Card";
import Badge from "components/ui/Badge";
import ErrorState from "components/ui/ErrorState";
import { PageLoader } from "components/ui/Spinner";
import ConfirmDialog from "components/common/ConfirmDialog";
import SpouseModal from "components/modals/SpouseModal";
import ChildModal from "components/modals/ChildModal";
import {
  useEmployee,
  useDeleteEmployee,
  useDeleteSpouse,
  useDeleteChild,
} from "hooks/useEmployees";
import { useAuth } from "hooks/useAuth";
import { formatCurrency, formatPhone, formatDate } from "utils/helpers";
import { reportService } from "services/employeeService";
import { saveAs } from "file-saver";
import toast from "react-hot-toast";
import { Spouse, Child } from "types";

const item = {
  hidden: { opacity: 0, y: 10 },
  show: { opacity: 1, y: 0 },
};

const EmployeeDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { isAdmin } = useAuth();
  const { data: employee, isLoading, isError, refetch } = useEmployee(id!);
  const deleteMutation = useDeleteEmployee();
  const deleteSpouseMutation = useDeleteSpouse(id!);
  const deleteChildMutation = useDeleteChild(id!);

  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);
  const [deleteSpouseConfirm, setDeleteSpouseConfirm] = useState(false);
  const [deleteChildId, setDeleteChildId] = useState<string | null>(null);
  const [spouseModal, setSpouseModal] = useState<{
    open: boolean;
    spouse?: Spouse;
  }>({ open: false });
  const [childModal, setChildModal] = useState<{
    open: boolean;
    child?: Child;
  }>({ open: false });
  const [downloadingCv, setDownloadingCv] = useState(false);

  if (isLoading) return <PageLoader />;
  if (isError || !employee) return <ErrorState onRetry={refetch} />;

  const handleDeleteEmployee = async () => {
    try {
      await deleteMutation.mutateAsync(id!);
      navigate("/employees");
    } catch {
      toast.error("Failed to delete employee");
    }
  };

  const handleDeleteSpouse = async () => {
    try {
      await deleteSpouseMutation.mutateAsync();
      setDeleteSpouseConfirm(false);
    } catch {
      toast.error("Failed to delete spouse");
    }
  };

  const handleDeleteChild = async () => {
    if (!deleteChildId) return;
    try {
      await deleteChildMutation.mutateAsync(deleteChildId);
      setDeleteChildId(null);
    } catch {
      toast.error("Failed to delete child");
    }
  };

  const handleDownloadCv = async () => {
    setDownloadingCv(true);
    try {
      const res = await reportService.downloadEmployeeCv(id!);
      const cvFilename = `${employee.name}_${employee.department}_${id}`
        .replace(/\s+/g, "_")
        .replace(/[^\w-]/g, "");
      saveAs(res.data, `${cvFilename}.pdf`);
      toast.success("CV downloaded");
    } catch {
      toast.error("Failed to download CV");
    } finally {
      setDownloadingCv(false);
    }
  };

  const infoItems = [
    {
      icon: HiIdentification,
      label: "NID",
      value: employee.nid,
    },
    {
      icon: HiPhone,
      label: "Phone",
      value: formatPhone(employee.phone),
    },
    {
      icon: HiOfficeBuilding,
      label: "Department",
      value: employee.department,
    },
    {
      icon: HiCurrencyBangladeshi,
      label: "Basic Salary",
      value: formatCurrency(employee.basicSalary),
    },
  ];

  return (
    <motion.div
      initial="hidden"
      animate="show"
      variants={{ show: { transition: { staggerChildren: 0.08 } } }}
      className="space-y-6"
    >
      {/* Header */}
      <motion.div
        variants={item}
        className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4"
      >
        <div className="flex items-center gap-3">
          <Button
            variant="ghost"
            size="sm"
            onClick={() => navigate("/employees")}
            icon={<HiArrowLeft className="h-4 w-4" />}
          >
            Back
          </Button>
          <div>
            <h1 className="text-2xl font-bold text-brand-900 dark:text-brand-100">
              {employee.name}
            </h1>
            <Badge color="green">{employee.department}</Badge>
          </div>
        </div>
        <div className="flex gap-2 flex-wrap">
          <Button
            variant="secondary"
            size="sm"
            onClick={handleDownloadCv}
            loading={downloadingCv}
            icon={<HiDocumentDownload className="h-4 w-4" />}
          >
            Download CV
          </Button>
          {isAdmin && (
            <>
              <Button
                variant="secondary"
                size="sm"
                onClick={() => navigate(`/employees/${id}/edit`)}
                icon={<HiPencil className="h-4 w-4" />}
              >
                Edit
              </Button>
              <Button
                variant="danger"
                size="sm"
                onClick={() => setShowDeleteConfirm(true)}
                icon={<HiTrash className="h-4 w-4" />}
              >
                Delete
              </Button>
            </>
          )}
        </div>
      </motion.div>

      {/* Personal info */}
      <motion.div variants={item}>
        <Card>
          <h3 className="text-base font-semibold text-brand-800 dark:text-brand-200 mb-4">
            Personal Information
          </h3>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            {infoItems.map((ii) => (
              <div key={ii.label} className="flex items-center gap-3">
                <div className="rounded-lg bg-brand-100 p-2 dark:bg-brand-800">
                  <ii.icon className="h-5 w-5 text-brand-600 dark:text-brand-400" />
                </div>
                <div>
                  <p className="text-xs text-brand-500 dark:text-brand-400">
                    {ii.label}
                  </p>
                  <p className="text-sm font-medium text-brand-900 dark:text-brand-100">
                    {ii.value}
                  </p>
                </div>
              </div>
            ))}
          </div>

          {/* Employee GUID */}
          <div className="mt-4 pt-4 border-t border-brand-100 dark:border-brand-800">
            <p className="text-xs text-brand-500 dark:text-brand-400 mb-1.5">Employee ID (GUID)</p>
            <div className="flex items-center gap-2">
              <code className="flex-1 rounded-lg bg-brand-50 dark:bg-brand-800 px-3 py-1.5 text-xs font-mono text-brand-700 dark:text-brand-300 break-all select-all">
                {employee.id}
              </code>
              <button
                onClick={() => {
                  navigator.clipboard.writeText(employee.id);
                  toast.success("Copied to clipboard");
                }}
                className="rounded-lg p-1.5 text-brand-500 hover:bg-brand-100 dark:hover:bg-brand-800 transition-colors flex-shrink-0"
                title="Copy GUID"
              >
                <HiClipboard className="h-4 w-4" />
              </button>
            </div>
          </div>
        </Card>
      </motion.div>

      {/* Spouse */}
      <motion.div variants={item}>
        <Card>
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-base font-semibold text-brand-800 dark:text-brand-200 flex items-center gap-2">
              <HiHeart className="h-5 w-5 text-pink-400" />
              Spouse Information
            </h3>
            {isAdmin && !employee.spouse && (
              <Button
                variant="secondary"
                size="sm"
                onClick={() => setSpouseModal({ open: true })}
                icon={<HiUserAdd className="h-4 w-4" />}
              >
                Add Spouse
              </Button>
            )}
          </div>

          {employee.spouse ? (
            <div className="flex items-center justify-between">
              <div className="flex items-center gap-3">
                <div className="flex h-10 w-10 items-center justify-center rounded-full bg-pink-100 dark:bg-pink-900/30">
                  <HiHeart className="h-5 w-5 text-pink-500" />
                </div>
                <div>
                  <p className="font-medium text-brand-900 dark:text-brand-100">
                    {employee.spouse.name}
                  </p>
                  <p className="text-xs text-brand-500 dark:text-brand-400 font-mono">
                    NID: {employee.spouse.nid}
                  </p>
                </div>
              </div>
              {isAdmin && (
                <div className="flex gap-1">
                  <button
                    onClick={() =>
                      setSpouseModal({
                        open: true,
                        spouse: employee.spouse!,
                      })
                    }
                    className="rounded-lg p-1.5 text-brand-500 hover:bg-brand-100 dark:hover:bg-brand-800 transition-colors"
                  >
                    <HiPencil className="h-4 w-4" />
                  </button>
                  <button
                    onClick={() => setDeleteSpouseConfirm(true)}
                    className="rounded-lg p-1.5 text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors"
                  >
                    <HiTrash className="h-4 w-4" />
                  </button>
                </div>
              )}
            </div>
          ) : (
            <p className="text-sm text-brand-400 dark:text-brand-500">
              No spouse record
            </p>
          )}
        </Card>
      </motion.div>

      {/* Children */}
      <motion.div variants={item}>
        <Card>
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-base font-semibold text-brand-800 dark:text-brand-200">
              Children ({employee.children.length})
            </h3>
            {isAdmin && (
              <Button
                variant="secondary"
                size="sm"
                onClick={() => setChildModal({ open: true })}
                icon={<HiPlus className="h-4 w-4" />}
              >
                Add Child
              </Button>
            )}
          </div>

          {employee.children.length > 0 ? (
            <div className="space-y-3">
              {employee.children.map((child) => (
                <div
                  key={child.id}
                  className="flex items-center justify-between rounded-lg border border-brand-100 p-3 dark:border-brand-800"
                >
                  <div>
                    <p className="font-medium text-brand-900 dark:text-brand-100">
                      {child.name}
                    </p>
                    <p className="text-xs text-brand-500 dark:text-brand-400">
                      Born: {formatDate(child.dateOfBirth)}
                    </p>
                  </div>
                  {isAdmin && (
                    <div className="flex gap-1">
                      <button
                        onClick={() =>
                          setChildModal({ open: true, child })
                        }
                        className="rounded-lg p-1.5 text-brand-500 hover:bg-brand-100 dark:hover:bg-brand-800 transition-colors"
                      >
                        <HiPencil className="h-4 w-4" />
                      </button>
                      <button
                        onClick={() => setDeleteChildId(child.id)}
                        className="rounded-lg p-1.5 text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 transition-colors"
                      >
                        <HiTrash className="h-4 w-4" />
                      </button>
                    </div>
                  )}
                </div>
              ))}
            </div>
          ) : (
            <p className="text-sm text-brand-400 dark:text-brand-500">
              No children records
            </p>
          )}
        </Card>
      </motion.div>

      {/* Modals */}
      <SpouseModal
        employeeId={id!}
        open={spouseModal.open}
        spouse={spouseModal.spouse}
        onClose={() => setSpouseModal({ open: false })}
      />
      <ChildModal
        employeeId={id!}
        open={childModal.open}
        child={childModal.child}
        onClose={() => setChildModal({ open: false })}
      />

      {/* Confirm dialogs */}
      <ConfirmDialog
        open={showDeleteConfirm}
        onClose={() => setShowDeleteConfirm(false)}
        onConfirm={handleDeleteEmployee}
        title="Delete Employee"
        message="This will permanently delete this employee and all their family data."
        loading={deleteMutation.isPending}
      />
      <ConfirmDialog
        open={deleteSpouseConfirm}
        onClose={() => setDeleteSpouseConfirm(false)}
        onConfirm={handleDeleteSpouse}
        title="Delete Spouse"
        message="This will remove the spouse record."
        loading={deleteSpouseMutation.isPending}
      />
      <ConfirmDialog
        open={!!deleteChildId}
        onClose={() => setDeleteChildId(null)}
        onConfirm={handleDeleteChild}
        title="Delete Child"
        message="This will remove this child record."
        loading={deleteChildMutation.isPending}
      />
    </motion.div>
  );
};

export default EmployeeDetailPage;
