import React from "react";
import Button from "components/ui/Button";
import Modal from "components/ui/Modal";

interface ConfirmDialogProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  title: string;
  message: string;
  loading?: boolean;
  confirmLabel?: string;
}

const ConfirmDialog: React.FC<ConfirmDialogProps> = ({
  open,
  onClose,
  onConfirm,
  title,
  message,
  loading = false,
  confirmLabel = "Delete",
}) => (
  <Modal open={open} onClose={onClose} title={title} size="sm">
    <p className="text-sm text-brand-600 dark:text-brand-400 mb-6">
      {message}
    </p>
    <div className="flex justify-end gap-3">
      <Button variant="ghost" onClick={onClose} disabled={loading}>
        Cancel
      </Button>
      <Button variant="danger" onClick={onConfirm} loading={loading}>
        {confirmLabel}
      </Button>
    </div>
  </Modal>
);

export default ConfirmDialog;
