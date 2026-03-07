import React from "react";
import { Outlet } from "react-router-dom";
import Navbar from "./Navbar";
import Footer from "./Footer";

const MainLayout: React.FC = () => {
  return (
    <div className="flex flex-col min-h-screen bg-brand-50 dark:bg-brand-950 transition-colors duration-300">
      <Navbar />
      <main className="flex-1 mx-auto w-full max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
};

export default MainLayout;
