import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { AxiosError } from "axios";
import toast from "react-hot-toast";
import Modal from "components/ui/Modal";
import Input from "components/ui/Input";
import Button from "components/ui/Button";
import { Spouse, ApiResponse } from "types";
import { useAddSpouse, useUpdateSpouse } from "hooks/useEmployees";
import { extractApiErrors } from "utils/helpers";

interface SpouseModalProps {
  employeeId: string;
  open: boolean;
  spouse?: Spouse;
  onClose: () => void;
}

const schema = z.object({
  name: z.string().min(1, "Name is required").max(200),
  nid: z
    .string()
    .min(1, "NID is required")
    .regex(/^\d{10}$|^\d{17}$/, "NID must be exactly 10 or 17 digits"),
});

type FormValues = z.infer<typeof schema>;

const SpouseModal: React.FC<SpouseModalProps> = ({
  employeeId,
  open,
  spouse,
  onClose,
}) => {
  const isEdit = !!spouse;
  const addMutation = useAddSpouse(employeeId);
  const updateMutation = useUpdateSpouse(employeeId);
  const [submitting, setSubmitting] = useState(false);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: { name: "", nid: "" },
  });

  useEffect(() => {
    if (open) {
      reset(spouse ? { name: spouse.name, nid: spouse.nid } : { name: "", nid: "" });
    }
  }, [open, spouse, reset]);

  const onSubmit = async (values: FormValues) => {
    setSubmitting(true);
    try {
      if (isEdit && spouse) {
        await updateMutation.mutateAsync({
          ...values,
          rowVersion: spouse.rowVersion,
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
      title={isEdit ? "Edit Spouse" : "Add Spouse"}
    >
      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <Input
          label="Spouse Name"
          placeholder="Fatema Begum"
          error={errors.name?.message}
          {...register("name")}
        />
        <Input
          label="NID"
          placeholder="9876543210"
          error={errors.nid?.message}
          {...register("nid")}
        />
        <div className="flex justify-end gap-3 pt-4">
          <Button variant="ghost" type="button" onClick={onClose} disabled={submitting}>
            Cancel
          </Button>
          <Button type="submit" loading={submitting}>
            {isEdit ? "Update" : "Add Spouse"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default SpouseModal;
