export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat("en-BD", {
    style: "currency",
    currency: "BDT",
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  }).format(amount);
}

export function formatDate(dateStr: string): string {
  return new Date(dateStr).toLocaleDateString("en-GB", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  });
}

export function formatPhone(phone: string): string {
  if (phone.startsWith("+880")) {
    return `+880 ${phone.slice(4, 6)} ${phone.slice(6, 10)}-${phone.slice(10)}`;
  }
  return phone;
}

export function extractApiErrors(
  errors: Record<string, string[]> | null
): string[] {
  if (!errors) return [];
  return Object.values(errors).flat();
}

export function classNames(...classes: (string | boolean | undefined)[]): string {
  return classes.filter(Boolean).join(" ");
}
