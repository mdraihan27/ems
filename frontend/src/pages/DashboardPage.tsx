import React from "react";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";
import {
  HiUserGroup,
  HiDocumentReport,
  HiOfficeBuilding,
  HiCurrencyBangladeshi,
  HiUsers,
  HiUserAdd,
} from "react-icons/hi";
import Card from "components/ui/Card";
import { CardSkeleton } from "components/ui/Skeleton";
import ErrorState from "components/ui/ErrorState";
import { useEmployees } from "hooks/useEmployees";
import { useAuth } from "hooks/useAuth";
import { useTheme } from "hooks/useTheme";
import { formatCurrency } from "utils/helpers";
import {
  PieChart,
  Pie,
  Cell,
  Legend,
  ResponsiveContainer,
  Tooltip,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
} from "recharts";

const CHART_COLORS = ["#4E7249", "#9EB8A2", "#304F2A", "#789278", "#3D5F38", "#C7D8C3"];

const container = {
  hidden: {},
  show: { transition: { staggerChildren: 0.06 } },
};

const item = {
  hidden: { opacity: 0, y: 12 },
  show: { opacity: 1, y: 0 },
};

const DashboardPage: React.FC = () => {
  const navigate = useNavigate();
  const { isAdmin } = useAuth();
  const { theme } = useTheme();
  const isDark = theme === "dark";

  // Theme-aware chart colors
  const labelColor  = isDark ? "#D8E9D4" : "#304F2A";
  const axisColor   = isDark ? "#9EB8A2" : "#568056";
  const gridColor   = isDark ? "#2F4232" : "#C7D8C3";

  const RADIAN = Math.PI / 180;
  const renderPieLabel = ({ cx, cy, midAngle, outerRadius, name, percent }: any) => {
    const r = outerRadius + 30;
    const x = cx + r * Math.cos(-midAngle * RADIAN);
    const y = cy + r * Math.sin(-midAngle * RADIAN);
    return (
      <text
        x={x}
        y={y}
        fill={labelColor}
        textAnchor={x > cx ? "start" : "end"}
        dominantBaseline="central"
        fontSize={11}
        fontFamily="Roboto, sans-serif"
      >
        {`${name} (${(percent * 100).toFixed(0)}%)`}
      </text>
    );
  };

  const { data, isLoading, isError, refetch } = useEmployees({
    page: 1,
    pageSize: 100,
  });

  if (isLoading) {
    return (
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {Array.from({ length: 8 }).map((_, i) => (
          <CardSkeleton key={i} />
        ))}
      </div>
    );
  }

  if (isError) {
    return <ErrorState onRetry={refetch} />;
  }

  const employees = data?.items ?? [];
  const totalEmployees = data?.pagination.totalCount ?? 0;

  /* Derive stats */
  const departments = employees.reduce<Record<string, number>>((acc, emp) => {
    acc[emp.department] = (acc[emp.department] || 0) + 1;
    return acc;
  }, {});
  const deptData = Object.entries(departments).map(([name, value]) => ({
    name,
    value,
  }));
  const totalSalary = employees.reduce((s, e) => s + e.basicSalary, 0);
  const avgSalary = totalEmployees > 0 ? totalSalary / totalEmployees : 0;
  const withSpouse = employees.filter((e) => e.spouse).length;
  const totalChildren = employees.reduce((s, e) => s + e.children.length, 0);

  /* Salary distribution by department */
  const salaryByDept = Object.entries(
    employees.reduce<Record<string, number>>((acc, emp) => {
      acc[emp.department] = (acc[emp.department] || 0) + emp.basicSalary;
      return acc;
    }, {})
  ).map(([name, total]) => ({ name, total }));

  const statCards = [
    {
      label: "Total Employees",
      value: totalEmployees,
      icon: HiUserGroup,
      color: "text-brand-600 dark:text-brand-400",
      bg: "bg-brand-100 dark:bg-brand-800",
    },
    {
      label: "Departments",
      value: deptData.length,
      icon: HiOfficeBuilding,
      color: "text-blue-600 dark:text-blue-400",
      bg: "bg-blue-100 dark:bg-blue-900/30",
    },
    {
      label: "Avg. Salary",
      value: formatCurrency(avgSalary),
      icon: HiCurrencyBangladeshi,
      color: "text-green-600 dark:text-green-400",
      bg: "bg-green-100 dark:bg-green-900/30",
    },
    {
      label: "Family Members",
      value: withSpouse + totalChildren,
      icon: HiUsers,
      color: "text-purple-600 dark:text-purple-400",
      bg: "bg-purple-100 dark:bg-purple-900/30",
    },
  ];

  return (
    <motion.div variants={container} initial="hidden" animate="show" className="space-y-6">
      {/* Page header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-brand-900 dark:text-brand-100">
            Dashboard
          </h1>
          <p className="text-sm text-brand-500 dark:text-brand-400">
            Overview of your employee registry
          </p>
        </div>
        {isAdmin && (
          <motion.div variants={item}>
            <button
              onClick={() => navigate("/employees/new")}
              className="inline-flex items-center gap-2 rounded-lg bg-brand-600 px-4 py-2 text-sm font-medium text-white hover:bg-brand-700 dark:bg-brand-500 dark:hover:bg-brand-600 transition-colors"
            >
              <HiUserAdd className="h-4 w-4" />
              Add Employee
            </button>
          </motion.div>
        )}
      </div>

      {/* Stat cards */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {statCards.map((stat) => (
          <motion.div key={stat.label} variants={item}>
            <Card>
              <div className="flex items-center gap-4">
                <div className={`rounded-xl p-3 ${stat.bg}`}>
                  <stat.icon className={`h-6 w-6 ${stat.color}`} />
                </div>
                <div>
                  <p className="text-sm text-brand-500 dark:text-brand-400">
                    {stat.label}
                  </p>
                  <p className="text-xl font-bold text-brand-900 dark:text-brand-100">
                    {stat.value}
                  </p>
                </div>
              </div>
            </Card>
          </motion.div>
        ))}
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Department distribution */}
        <motion.div variants={item}>
          <Card>
            <h3 className="text-base font-semibold text-brand-800 dark:text-brand-200 mb-4">
              Department Distribution
            </h3>
            {deptData.length > 0 ? (
              <ResponsiveContainer width="100%" height={300}>
                <PieChart margin={{ top: 16, right: 40, bottom: 16, left: 40 }}>
                  <Pie
                    data={deptData}
                    dataKey="value"
                    nameKey="name"
                    cx="50%"
                    cy="50%"
                    outerRadius={85}
                    innerRadius={46}
                    paddingAngle={3}
                    label={renderPieLabel}
                    labelLine={{ stroke: labelColor, strokeWidth: 1 }}
                  >
                    {deptData.map((_, idx) => (
                      <Cell
                        key={idx}
                        fill={CHART_COLORS[idx % CHART_COLORS.length]}
                      />
                    ))}
                  </Pie>
                  <Tooltip />
                </PieChart>
              </ResponsiveContainer>
            ) : (
              <p className="text-sm text-brand-400">No data</p>
            )}
          </Card>
        </motion.div>

        {/* Salary by department */}
        <motion.div variants={item}>
          <Card>
            <h3 className="text-base font-semibold text-brand-800 dark:text-brand-200 mb-4">
              Total Salary by Department
            </h3>
            {salaryByDept.length > 0 ? (
              <ResponsiveContainer width="100%" height={260}>
                <BarChart data={salaryByDept}>
                  <CartesianGrid strokeDasharray="3 3" stroke={gridColor} />
                  <XAxis
                    dataKey="name"
                    tick={{ fontSize: 12, fill: axisColor }}
                    stroke={axisColor}
                  />
                  <YAxis tick={{ fontSize: 12, fill: axisColor }} stroke={axisColor} />
                  <Tooltip
                    formatter={(value: any) => formatCurrency(Number(value))}
                  />
                  <Bar dataKey="total" radius={[6, 6, 0, 0]}>
                    {salaryByDept.map((_, idx) => (
                      <Cell
                        key={idx}
                        fill={CHART_COLORS[idx % CHART_COLORS.length]}
                      />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            ) : (
              <p className="text-sm text-brand-400">No data</p>
            )}
          </Card>
        </motion.div>
      </div>

      {/* Quick nav cards */}
      <motion.div variants={item}>
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
          <Card hoverable onClick={() => navigate("/employees")}>
            <div className="flex items-center gap-3">
              <HiUserGroup className="h-6 w-6 text-brand-600 dark:text-brand-400" />
              <div>
                <p className="font-medium text-brand-800 dark:text-brand-200">
                  Manage Employees
                </p>
                <p className="text-xs text-brand-500 dark:text-brand-400">
                  View, search, and manage employee records
                </p>
              </div>
            </div>
          </Card>
          <Card hoverable onClick={() => navigate("/reports")}>
            <div className="flex items-center gap-3">
              <HiDocumentReport className="h-6 w-6 text-brand-600 dark:text-brand-400" />
              <div>
                <p className="font-medium text-brand-800 dark:text-brand-200">
                  Reports
                </p>
                <p className="text-xs text-brand-500 dark:text-brand-400">
                  Download PDF reports and CVs
                </p>
              </div>
            </div>
          </Card>
          {isAdmin && (
            <Card hoverable onClick={() => navigate("/employees/new")}>
              <div className="flex items-center gap-3">
                <HiUserAdd className="h-6 w-6 text-brand-600 dark:text-brand-400" />
                <div>
                  <p className="font-medium text-brand-800 dark:text-brand-200">
                    Add Employee
                  </p>
                  <p className="text-xs text-brand-500 dark:text-brand-400">
                    Register new employee with family info
                  </p>
                </div>
              </div>
            </Card>
          )}
        </div>
      </motion.div>

      {/* Recent employees */}
      <motion.div variants={item}>
        <Card>
          <h3 className="text-base font-semibold text-brand-800 dark:text-brand-200 mb-4">
            Recent Employees
          </h3>
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-brand-200 dark:border-brand-700">
                  <th className="py-2 pr-4 text-left font-medium text-brand-500 dark:text-brand-400">
                    Name
                  </th>
                  <th className="py-2 pr-4 text-left font-medium text-brand-500 dark:text-brand-400">
                    Department
                  </th>
                  <th className="py-2 pr-4 text-left font-medium text-brand-500 dark:text-brand-400">
                    Salary
                  </th>
                </tr>
              </thead>
              <tbody>
                {employees.slice(0, 5).map((emp) => (
                  <tr
                    key={emp.id}
                    onClick={() => navigate(`/employees/${emp.id}`)}
                    className="border-b border-brand-100 dark:border-brand-800 cursor-pointer hover:bg-brand-50 dark:hover:bg-brand-800/50 transition-colors"
                  >
                    <td className="py-2.5 pr-4 font-medium text-brand-900 dark:text-brand-100">
                      {emp.name}
                    </td>
                    <td className="py-2.5 pr-4 text-brand-600 dark:text-brand-400">
                      {emp.department}
                    </td>
                    <td className="py-2.5 pr-4 text-brand-600 dark:text-brand-400">
                      {formatCurrency(emp.basicSalary)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </Card>
      </motion.div>
    </motion.div>
  );
};

export default DashboardPage;
