import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { AxiosError } from "axios";
import toast from "react-hot-toast";
import Modal from "components/ui/Modal";
import Input from "components/ui/Input";
import Button from "components/ui/Button";
import { Child, ApiResponse } from "types";
import { useAddChild, useUpdateChild } from "hooks/useEmployees";
import { extractApiErrors } from "utils/helpers";

interface ChildModalProps {
  employeeId: string;
  open: boolean;
  child?: Child;
  onClose: () => void;
}

const schema = z.object({
  name: z.string().min(1, "Name is required").max(200),
  dateOfBirth: z.string().min(1, "Date of birth is required").refine(
    (val) => new Date(val) <= new Date(),
    "Date of birth cannot be in the future"
  ),
});

type FormValues = z.infer<typeof schema>;

const ChildModal: React.FC<ChildModalProps> = ({
  employeeId,
  open,
  child,
  onClose,
}) => {
  const isEdit = !!child;
  const addMutation = useAddChild(employeeId);
  const updateMutation = useUpdateChild(employeeId);
  const [submitting, setSubmitting] = useState(false);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: { name: "", dateOfBirth: "" },
  });

  useEffect(() => {
    if (open) {
      reset(
        child
          ? { name: child.name, dateOfBirth: child.dateOfBirth.split("T")[0] }
          : { name: "", dateOfBirth: "" }
      );
    }
  }, [open, child, reset]);

  const onSubmit = async (values: FormValues) => {
    setSubmitting(true);
    try {
      if (isEdit && child) {
        await updateMutation.mutateAsync({
          childId: child.id,
          data: { ...values, rowVersion: child.rowVersion },
        });
      } else {
        await addMutation.mutateAsync(values);
      }
      onClose();
    } catch (err) {
      const axiosErr = err as AxiosError<ApiResponse<null>>;
      const serverErrors = extractApiErrors(
        axiosErr.response?.data?.errors ?? null
      );
      toast.error(
        serverErrors.length > 0
          ? serverErrors.join(", ")
          : axiosErr.response?.data?.message || "Operation failed"
      );
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      title={isEdit ? "Edit Child" : "Add Child"}
    >
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <Input
          label="Child Name"
          placeholder="Rahim Hasan"
          error={errors.name?.message}
          {...register("name")}
        />
        <Input
          label="Date of Birth"
          type="date"
          error={errors.dateOfBirth?.message}
          {...register("dateOfBirth")}
        />
        <div className="flex justify-end gap-3 pt-4">
          <Button variant="ghost" type="button" onClick={onClose} disabled={submitting}>
            Cancel
          </Button>
          <Button type="submit" loading={submitting}>
            {isEdit ? "Update" : "Add Child"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default ChildModal;
