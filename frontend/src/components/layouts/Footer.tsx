import React from "react";

const Footer: React.FC = () => (
  <footer className="border-t border-brand-200 dark:border-brand-800 bg-white/60 dark:bg-brand-950/60 backdrop-blur-md mt-auto">
    <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 py-4 flex flex-col sm:flex-row items-center justify-between gap-2">
      <p className="text-xs text-brand-500 dark:text-brand-400">
        &copy; {new Date().getFullYear()} Employee Management System &mdash; Bangladesh
      </p>
      <p className="text-xs text-brand-400 dark:text-brand-500">
        Built with React &amp; .NET 10
      </p>
    </div>
  </footer>
);

export default Footer;
