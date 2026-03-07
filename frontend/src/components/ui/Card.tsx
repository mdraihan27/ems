import React from "react";
import { motion } from "framer-motion";

interface CardProps {
  children: React.ReactNode;
  className?: string;
  hoverable?: boolean;
  onClick?: () => void;
}

const Card: React.FC<CardProps> = ({
  children,
  className = "",
  hoverable = false,
  onClick,
}) => {
  return (
    <motion.div
      initial={{ opacity: 0, y: 8 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.3 }}
      whileHover={hoverable ? { y: -2, boxShadow: "0 8px 30px rgba(0,0,0,0.08)" } : undefined}
      onClick={onClick}
      className={`rounded-xl border border-brand-200 bg-white p-6 shadow-sm dark:border-brand-800 dark:bg-brand-900 ${
        hoverable ? "cursor-pointer" : ""
      } ${className}`}
    >
      {children}
    </motion.div>
  );
};

export default Card;
