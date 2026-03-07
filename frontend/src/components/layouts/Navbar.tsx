import React, { useState, useRef, useEffect } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { motion, AnimatePresence } from "framer-motion";
import {
  HiMenu,
  HiX,
  HiSun,
  HiMoon,
  HiLogout,
  HiUser,
  HiHome,
  HiUserGroup,
  HiDocumentReport,
  HiLogin,
  HiUserAdd,
} from "react-icons/hi";
import { useAuth } from "hooks/useAuth";
import { useTheme } from "hooks/useTheme";

const navLinks = [
  { to: "/dashboard", label: "Dashboard", icon: HiHome },
  { to: "/employees", label: "Employees", icon: HiUserGroup },
  { to: "/reports", label: "Reports", icon: HiDocumentReport },
];

const Navbar: React.FC = () => {
  const { auth, logout, isAdmin } = useAuth();
  const { theme, toggleTheme } = useTheme();
  const location = useLocation();
  const navigate = useNavigate();
  const [mobileOpen, setMobileOpen] = useState(false);
  const [profileOpen, setProfileOpen] = useState(false);
  const profileRef = useRef<HTMLDivElement>(null);

  /* Close profile dropdown on outside click */
  useEffect(() => {
    const handler = (e: MouseEvent) => {
      if (profileRef.current && !profileRef.current.contains(e.target as Node)) {
        setProfileOpen(false);
      }
    };
    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, []);

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  const themeBtn = (
    <motion.button
      whileTap={{ scale: 0.9 }}
      onClick={toggleTheme}
      className="rounded-lg p-2 text-brand-600 hover:bg-brand-100 dark:text-brand-400 dark:hover:bg-brand-800 transition-colors"
      aria-label="Toggle theme"
    >
      {theme === "light" ? <HiMoon className="h-5 w-5" /> : <HiSun className="h-5 w-5" />}
    </motion.button>
  );

  /* ── Unauthenticated header ── */
  if (!auth) {
    return (
      <nav className="sticky top-0 z-40 border-b border-brand-200 bg-white/80 backdrop-blur-md dark:border-brand-800 dark:bg-brand-950/80">
        <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <div className="flex h-16 items-center justify-between">
            <Link to="/" className="flex items-center gap-2">
              <img
                src="/assets/images/ems_logo.png"
                alt="EMS Logo"
                className="h-9 w-auto dark:brightness-[1.8] dark:contrast-75"
              />
              <span className="hidden sm:block text-lg font-semibold text-brand-900 dark:text-brand-100">
                EMS
              </span>
            </Link>
            <div className="flex items-center gap-2">
              {themeBtn}
              <Link
                to="/login"
                className={`flex items-center gap-1.5 rounded-lg px-3 py-2 text-sm font-medium transition-colors ${
                  location.pathname === "/login"
                    ? "bg-brand-100 text-brand-800 dark:bg-brand-800 dark:text-brand-100"
                    : "text-brand-600 hover:bg-brand-100 dark:text-brand-400 dark:hover:bg-brand-800"
                }`}
              >
                <HiLogin className="h-4 w-4" />
                Sign In
              </Link>
              <Link
                to="/register"
                className={`flex items-center gap-1.5 rounded-lg px-3 py-2 text-sm font-medium transition-colors ${
                  location.pathname === "/register"
                    ? "bg-brand-100 text-brand-800 dark:bg-brand-800 dark:text-brand-100"
                    : "text-brand-600 hover:bg-brand-100 dark:text-brand-400 dark:hover:bg-brand-800"
                }`}
              >
                <HiUserAdd className="h-4 w-4" />
                Register
              </Link>
            </div>
          </div>
        </div>
      </nav>
    );
  }

  /* ── Authenticated header ── */
  return (
    <nav className="sticky top-0 z-40 border-b border-brand-200 bg-white/80 backdrop-blur-md dark:border-brand-800 dark:bg-brand-950/80">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div className="flex h-16 items-center justify-between">
          {/* Logo */}
          <Link to="/dashboard" className="flex items-center gap-2">
            <img
              src="/assets/images/ems_logo.png"
              alt="EMS Logo"
              className="h-9 w-auto dark:brightness-[1.8] dark:contrast-75"
            />
            <span className="hidden sm:block text-lg font-semibold text-brand-900 dark:text-brand-100">
              EMS
            </span>
          </Link>

          {/* Desktop nav links */}
          <div className="hidden md:flex items-center gap-1">
            {navLinks.map((link) => {
              const active = location.pathname.startsWith(link.to);
              return (
                <Link
                  key={link.to}
                  to={link.to}
                  className={`flex items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium transition-colors ${
                    active
                      ? "bg-brand-100 text-brand-800 dark:bg-brand-800 dark:text-brand-100"
                      : "text-brand-600 hover:bg-brand-50 dark:text-brand-400 dark:hover:bg-brand-900"
                  }`}
                >
                  <link.icon className="h-4 w-4" />
                  {link.label}
                </Link>
              );
            })}
          </div>

          {/* Right side */}
          <div className="flex items-center gap-2">
            {/* Theme toggle */}
            <motion.button
              whileTap={{ scale: 0.9 }}
              onClick={toggleTheme}
              className="rounded-lg p-2 text-brand-600 hover:bg-brand-100 dark:text-brand-400 dark:hover:bg-brand-800 transition-colors"
              aria-label="Toggle theme"
            >
              {theme === "light" ? (
                <HiMoon className="h-5 w-5" />
              ) : (
                <HiSun className="h-5 w-5" />
              )}
            </motion.button>

            {/* Profile dropdown */}
            <div className="relative" ref={profileRef}>
              <button
                onClick={() => setProfileOpen((p) => !p)}
                className="flex items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium text-brand-600 hover:bg-brand-100 dark:text-brand-400 dark:hover:bg-brand-800 transition-colors"
              >
                <HiUser className="h-5 w-5" />
                <span className="hidden sm:block max-w-[120px] truncate">
                  {auth?.email}
                </span>
              </button>

              <AnimatePresence>
                {profileOpen && (
                  <motion.div
                    initial={{ opacity: 0, y: -8 }}
                    animate={{ opacity: 1, y: 0 }}
                    exit={{ opacity: 0, y: -8 }}
                    transition={{ duration: 0.15 }}
                    className="absolute right-0 mt-2 w-56 rounded-xl border border-brand-200 bg-white py-2 shadow-lg dark:border-brand-700 dark:bg-brand-900"
                  >
                    <div className="px-4 py-2 border-b border-brand-100 dark:border-brand-800">
                      <p className="text-sm font-medium text-brand-900 dark:text-brand-100 truncate">
                        {auth?.email}
                      </p>
                      <p className="text-xs text-brand-500 dark:text-brand-400">
                        {isAdmin ? "Admin" : "Viewer"}
                      </p>
                    </div>
                    <button
                      onClick={handleLogout}
                      className="flex w-full items-center gap-2 px-4 py-2 text-sm text-red-600 hover:bg-red-50 dark:text-red-400 dark:hover:bg-red-900/20 transition-colors"
                    >
                      <HiLogout className="h-4 w-4" />
                      Sign out
                    </button>
                  </motion.div>
                )}
              </AnimatePresence>
            </div>

            {/* Mobile menu button */}
            <button
              onClick={() => setMobileOpen((p) => !p)}
              className="md:hidden rounded-lg p-2 text-brand-600 hover:bg-brand-100 dark:text-brand-400 dark:hover:bg-brand-800 transition-colors"
            >
              {mobileOpen ? (
                <HiX className="h-5 w-5" />
              ) : (
                <HiMenu className="h-5 w-5" />
              )}
            </button>
          </div>
        </div>
      </div>

      {/* Mobile nav */}
      <AnimatePresence>
        {mobileOpen && (
          <motion.div
            initial={{ height: 0, opacity: 0 }}
            animate={{ height: "auto", opacity: 1 }}
            exit={{ height: 0, opacity: 0 }}
            transition={{ duration: 0.2 }}
            className="md:hidden overflow-hidden border-t border-brand-200 dark:border-brand-800"
          >
            <div className="space-y-1 px-4 py-3">
              {navLinks.map((link) => {
                const active = location.pathname.startsWith(link.to);
                return (
                  <Link
                    key={link.to}
                    to={link.to}
                    onClick={() => setMobileOpen(false)}
                    className={`flex items-center gap-2 rounded-lg px-3 py-2 text-sm font-medium transition-colors ${
                      active
                        ? "bg-brand-100 text-brand-800 dark:bg-brand-800 dark:text-brand-100"
                        : "text-brand-600 hover:bg-brand-50 dark:text-brand-400 dark:hover:bg-brand-900"
                    }`}
                  >
                    <link.icon className="h-4 w-4" />
                    {link.label}
                  </Link>
                );
              })}
            </div>
          </motion.div>
        )}
      </AnimatePresence>
    </nav>
  );
};

export default Navbar;
