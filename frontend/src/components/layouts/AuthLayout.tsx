import React from "react";
import { Outlet } from "react-router-dom";
import { motion } from "framer-motion";
import Navbar from "./Navbar";
import Footer from "./Footer";

const AuthLayout: React.FC = () => {
  return (
    <div className="flex flex-col min-h-screen bg-brand-50 dark:bg-brand-950 transition-colors duration-300">
      <Navbar />
      <main className="flex-1 flex items-center justify-center p-4">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.4 }}
          className="w-full max-w-md"
        >
          <Outlet />
        </motion.div>
      </main>
      <Footer />
    </div>
  );
};

export default AuthLayout;
