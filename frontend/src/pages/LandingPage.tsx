import React from "react";
import { Link } from "react-router-dom";
import { motion } from "framer-motion";
import {
  HiUserGroup,
  HiShieldCheck,
  HiDocumentReport,
  HiSearch,
  HiHeart,
  HiLightningBolt,
  HiSun,
  HiMoon,
  HiArrowRight,
  HiChevronDown,
} from "react-icons/hi";
import { useTheme } from "hooks/useTheme";
import { useAuth } from "hooks/useAuth";

/* Stagger animation helpers */
const containerVariant = {
  hidden: {},
  show: { transition: { staggerChildren: 0.1, delayChildren: 0.2 } },
};

const fadeUp = {
  hidden: { opacity: 0, y: 30 },
  show: { opacity: 1, y: 0, transition: { duration: 0.6, ease: "easeOut" as const } },
};

const scaleIn = {
  hidden: { opacity: 0, scale: 0.9 },
  show: { opacity: 1, scale: 1, transition: { duration: 0.5, ease: "easeOut" as const } },
};

const features = [
  {
    icon: HiUserGroup,
    title: "Employee Registry",
    desc: "Complete employee lifecycle management with detailed profiles, NID tracking, and department categorization.",
  },
  {
    icon: HiHeart,
    title: "Family Management",
    desc: "Maintain spouse and children records linked to each employee for a comprehensive family registry.",
  },
  {
    icon: HiSearch,
    title: "Smart Search",
    desc: "Lightning-fast search across names, NID numbers, and departments with real-time results.",
  },
  {
    icon: HiDocumentReport,
    title: "PDF Reports",
    desc: "Generate professional employee list reports and individual CV documents instantly.",
  },
  {
    icon: HiShieldCheck,
    title: "Role-Based Access",
    desc: "Secure Admin and Viewer roles with fine-grained permissions for every operation.",
  },
  {
    icon: HiLightningBolt,
    title: "Real-Time Updates",
    desc: "Optimistic UI with background data synchronization and concurrency conflict detection.",
  },
];

const stats = [
  { label: "Uptime Guarantee", value: "99.9%" },
  { label: "Data Encrypted", value: "256-bit" },
  { label: "Response Time", value: "<200ms" },
  { label: "Bangladesh Optimized", value: "100%" },
];

const LandingPage: React.FC = () => {
  const { theme, toggleTheme } = useTheme();
  const { isAuthenticated } = useAuth();

  return (
    <div className="min-h-screen bg-brand-50 dark:bg-brand-950 transition-colors duration-500 overflow-hidden">
      {/* ─── Top Navbar ─── */}
      <nav className="fixed top-0 left-0 right-0 z-50 border-b border-brand-200/60 bg-white/70 backdrop-blur-xl dark:border-brand-800/60 dark:bg-brand-950/70">
        <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-4 sm:px-6 lg:px-8">
          {/* Logo */}
          <Link to="/" className="flex items-center gap-3">
            <img
              src="/assets/images/ems_logo.png"
              alt="EMS Logo"
              className="h-10 w-auto dark:brightness-[1.8] dark:contrast-75"
            />
            <span className="text-xl font-bold tracking-tight text-brand-900 dark:text-brand-100">
              EMS
            </span>
          </Link>

          {/* Right actions */}
          <div className="flex items-center gap-2">
            <motion.button
              whileTap={{ scale: 0.9 }}
              onClick={toggleTheme}
              className="rounded-xl p-2.5 text-brand-600 hover:bg-brand-100 dark:text-brand-400 dark:hover:bg-brand-800 transition-colors"
              aria-label="Toggle theme"
            >
              {theme === "light" ? <HiMoon className="h-5 w-5" /> : <HiSun className="h-5 w-5" />}
            </motion.button>

            {isAuthenticated ? (
              <Link
                to="/dashboard"
                className="rounded-xl bg-brand-600 px-5 py-2.5 text-sm font-semibold text-white shadow-md shadow-brand-600/20 hover:bg-brand-700 dark:bg-brand-500 dark:shadow-brand-500/20 dark:hover:bg-brand-600 transition-all"
              >
                Dashboard
              </Link>
            ) : (
              <div className="flex items-center gap-2">
                <Link
                  to="/login"
                  className="rounded-xl px-4 py-2.5 text-sm font-medium text-brand-700 hover:bg-brand-100 dark:text-brand-300 dark:hover:bg-brand-800 transition-colors"
                >
                  Sign In
                </Link>
                <Link
                  to="/register"
                  className="rounded-xl bg-brand-600 px-5 py-2.5 text-sm font-semibold text-white shadow-md shadow-brand-600/20 hover:bg-brand-700 dark:bg-brand-500 dark:shadow-brand-500/20 dark:hover:bg-brand-600 transition-all"
                >
                  Get Started
                </Link>
              </div>
            )}
          </div>
        </div>
      </nav>

      {/* ─── Hero Section ─── */}
      <section className="relative pt-32 pb-20 sm:pt-40 sm:pb-28 lg:pt-48 lg:pb-36">
        {/* Decorative background blobs */}
        <div className="absolute inset-0 overflow-hidden pointer-events-none">
          <div className="absolute -top-40 -right-40 h-[500px] w-[500px] rounded-full bg-brand-400/10 blur-3xl dark:bg-brand-500/5" />
          <div className="absolute top-1/3 -left-32 h-[400px] w-[400px] rounded-full bg-brand-300/15 blur-3xl dark:bg-brand-600/5" />
          <div className="absolute bottom-0 right-1/4 h-[350px] w-[350px] rounded-full bg-brand-500/8 blur-3xl dark:bg-brand-400/5" />
        </div>

        <div className="relative mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <motion.div
            variants={containerVariant}
            initial="hidden"
            animate="show"
            className="text-center"
          >
            {/* Logo hero */}
            <motion.div variants={scaleIn} className="mb-8">
              <div className="relative mx-auto inline-block">
                <div className="absolute inset-0 rounded-3xl bg-brand-500/10 blur-2xl scale-125 dark:bg-brand-400/10" />
                <img
                  src="/assets/images/ems_logo.png"
                  alt="EMS"
                  className="relative h-28 w-auto sm:h-36 lg:h-44 drop-shadow-lg dark:brightness-[1.8] dark:contrast-75"
                />
              </div>
            </motion.div>

            {/* Headline */}
            <motion.h1
              variants={fadeUp}
              className="mx-auto max-w-4xl text-4xl font-extrabold leading-tight tracking-tight text-brand-900 dark:text-brand-100 sm:text-5xl lg:text-6xl"
            >
              Employee & Family{" "}
              <span className="relative">
                <span className="relative z-10 text-brand-600 dark:text-brand-400">
                  Registry System
                </span>
                <span className="absolute bottom-1 left-0 right-0 h-3 bg-brand-300/40 dark:bg-brand-600/30 -z-0 rounded-sm" />
              </span>
            </motion.h1>

            <motion.p
              variants={fadeUp}
              className="mx-auto mt-6 max-w-2xl text-lg leading-relaxed text-brand-600 dark:text-brand-400 sm:text-xl"
            >
              A modern, enterprise-grade platform for managing employee records,
              family data, and organizational reports — built for Bangladesh.
            </motion.p>

            {/* CTA Buttons */}
            <motion.div
              variants={fadeUp}
              className="mt-10 flex flex-col sm:flex-row items-center justify-center gap-4"
            >
              <Link
                to={isAuthenticated ? "/dashboard" : "/register"}
                className="group inline-flex items-center gap-2 rounded-2xl bg-brand-600 px-8 py-4 text-base font-semibold text-white shadow-lg shadow-brand-600/25 hover:bg-brand-700 hover:shadow-brand-600/35 dark:bg-brand-500 dark:shadow-brand-500/25 dark:hover:bg-brand-600 transition-all duration-300"
              >
                {isAuthenticated ? "Go to Dashboard" : "Get Started Free"}
                <HiArrowRight className="h-5 w-5 transition-transform group-hover:translate-x-1" />
              </Link>
              <Link
                to="/login"
                className="inline-flex items-center gap-2 rounded-2xl border-2 border-brand-300 bg-white/60 px-8 py-4 text-base font-semibold text-brand-700 backdrop-blur-sm hover:border-brand-400 hover:bg-white dark:border-brand-700 dark:bg-brand-900/40 dark:text-brand-300 dark:hover:border-brand-600 dark:hover:bg-brand-900/60 transition-all duration-300"
              >
                Sign In
              </Link>
            </motion.div>

            {/* Scroll hint */}
            <motion.div
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 1.5, duration: 0.6 }}
              className="mt-16"
            >
              <motion.div
                animate={{ y: [0, 8, 0] }}
                transition={{ repeat: Infinity, duration: 2, ease: "easeInOut" }}
              >
                <HiChevronDown className="mx-auto h-6 w-6 text-brand-400 dark:text-brand-600" />
              </motion.div>
            </motion.div>
          </motion.div>
        </div>
      </section>

      {/* ─── Stats Strip ─── */}
      <section className="border-y border-brand-200/60 bg-white/50 backdrop-blur-lg dark:border-brand-800/60 dark:bg-brand-900/30">
        <div className="mx-auto max-w-7xl px-4 py-10 sm:px-6 lg:px-8">
          <motion.div
            variants={containerVariant}
            initial="hidden"
            whileInView="show"
            viewport={{ once: true, amount: 0.5 }}
            className="grid grid-cols-2 gap-6 sm:grid-cols-4"
          >
            {stats.map((stat) => (
              <motion.div key={stat.label} variants={fadeUp} className="text-center">
                <p className="text-2xl font-bold text-brand-800 dark:text-brand-200 sm:text-3xl">
                  {stat.value}
                </p>
                <p className="mt-1 text-sm text-brand-500 dark:text-brand-400">
                  {stat.label}
                </p>
              </motion.div>
            ))}
          </motion.div>
        </div>
      </section>

      {/* ─── Features Grid ─── */}
      <section className="py-20 sm:py-28 lg:py-32">
        <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <motion.div
            variants={containerVariant}
            initial="hidden"
            whileInView="show"
            viewport={{ once: true, amount: 0.2 }}
          >
            <motion.div variants={fadeUp} className="text-center mb-16">
              <p className="text-sm font-semibold uppercase tracking-widest text-brand-500 dark:text-brand-400">
                Everything You Need
              </p>
              <h2 className="mt-3 text-3xl font-bold text-brand-900 dark:text-brand-100 sm:text-4xl">
                Powerful Features
              </h2>
              <p className="mx-auto mt-4 max-w-xl text-brand-600 dark:text-brand-400">
                A comprehensive suite of tools designed for efficient employee
                and family data management.
              </p>
            </motion.div>

            <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
              {features.map((feat) => (
                <motion.div
                  key={feat.title}
                  variants={fadeUp}
                  whileHover={{ y: -4, transition: { duration: 0.2 } }}
                  className="group rounded-2xl border border-brand-200/80 bg-white p-7 shadow-sm hover:shadow-lg hover:border-brand-300 dark:border-brand-800/80 dark:bg-brand-900 dark:hover:border-brand-700 transition-all duration-300"
                >
                  <div className="mb-4 inline-flex rounded-xl bg-brand-100 p-3 text-brand-600 group-hover:bg-brand-200 dark:bg-brand-800 dark:text-brand-400 dark:group-hover:bg-brand-700 transition-colors">
                    <feat.icon className="h-6 w-6" />
                  </div>
                  <h3 className="text-lg font-semibold text-brand-900 dark:text-brand-100">
                    {feat.title}
                  </h3>
                  <p className="mt-2 text-sm leading-relaxed text-brand-500 dark:text-brand-400">
                    {feat.desc}
                  </p>
                </motion.div>
              ))}
            </div>
          </motion.div>
        </div>
      </section>

      {/* ─── CTA Banner ─── */}
      <section className="py-20 sm:py-24">
        <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <motion.div
            initial={{ opacity: 0, y: 30 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{ duration: 0.6 }}
            className="relative overflow-hidden rounded-3xl bg-gradient-to-br from-brand-600 via-brand-700 to-brand-800 dark:from-brand-700 dark:via-brand-800 dark:to-brand-900 p-10 sm:p-16 text-center"
          >
            {/* Decorative circles */}
            <div className="absolute -top-16 -right-16 h-64 w-64 rounded-full bg-white/5" />
            <div className="absolute -bottom-20 -left-20 h-80 w-80 rounded-full bg-white/5" />

            <div className="relative">
              <img
                src="/assets/images/ems_logo.png"
                alt="EMS"
                className="mx-auto mb-6 h-16 w-auto brightness-[2] contrast-75 opacity-90"
              />
              <h2 className="text-3xl font-bold text-white sm:text-4xl">
                Ready to streamline your workforce management?
              </h2>
              <p className="mx-auto mt-4 max-w-xl text-brand-200/80">
                Join organizations across Bangladesh using EMS to manage
                employee records, family data, and generate professional reports
                effortlessly.
              </p>
              <div className="mt-8 flex flex-col sm:flex-row items-center justify-center gap-4">
                <Link
                  to={isAuthenticated ? "/dashboard" : "/register"}
                  className="group inline-flex items-center gap-2 rounded-2xl bg-white px-8 py-4 text-base font-semibold text-brand-700 shadow-lg hover:bg-brand-50 transition-all duration-300"
                >
                  {isAuthenticated ? "Open Dashboard" : "Create Free Account"}
                  <HiArrowRight className="h-5 w-5 transition-transform group-hover:translate-x-1" />
                </Link>
                <Link
                  to="/login"
                  className="inline-flex items-center gap-2 rounded-2xl border-2 border-white/30 px-8 py-4 text-base font-semibold text-white backdrop-blur-sm hover:border-white/60 hover:bg-white/10 transition-all duration-300"
                >
                  Sign In
                </Link>
              </div>
            </div>
          </motion.div>
        </div>
      </section>

      {/* ─── Footer ─── */}
      <footer className="border-t border-brand-200/60 bg-white/30 dark:border-brand-800/60 dark:bg-brand-950/50">
        <div className="mx-auto max-w-7xl px-4 py-10 sm:px-6 lg:px-8">
          <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
            <div className="flex items-center gap-3">
              <img
                src="/assets/images/ems_logo.png"
                alt="EMS"
                className="h-8 w-auto dark:brightness-[1.8] dark:contrast-75"
              />
              <span className="text-sm font-semibold text-brand-800 dark:text-brand-200">
                Employee Management System
              </span>
            </div>
            <p className="text-xs text-brand-400 dark:text-brand-500">
              &copy; {new Date().getFullYear()} EMS — Employee &amp; Family Registry.
              All rights reserved.
            </p>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default LandingPage;
