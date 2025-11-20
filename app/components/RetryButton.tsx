import React, { useState } from 'react';
import { RefreshCw } from 'lucide-react';

type RetryButtonVariant = "primary" | "secondary" | "danger" | "outline" | "ghost";
type RetryButtonSize = "sm" | "md" | "lg";

interface RetryButtonProps {
    onClick?: React.MouseEventHandler<HTMLButtonElement>;
    loading?: boolean;
    disabled?: boolean;
    children?: React.ReactNode;
    variant?: RetryButtonVariant;
    size?: RetryButtonSize;
    className?: string;
}

const RetryButton: React.FC<RetryButtonProps> = ({
                         onClick,
                         loading = false,
                         disabled = false,
                         children = "Retry",
                         variant = "primary",
                         size = "md",
                         className = ""
                     }) => {
    const baseStyles = "inline-flex items-center justify-center gap-2 font-medium rounded-lg transition-all duration-200 focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed";

    const variants = {
        primary: "bg-blue-600 hover:bg-blue-700 text-white focus:ring-blue-500 active:scale-95",
        secondary: "bg-gray-200 hover:bg-gray-300 text-gray-800 focus:ring-gray-500 active:scale-95",
        danger: "bg-red-600 hover:bg-red-700 text-white focus:ring-red-500 active:scale-95",
        outline: "border-2 border-blue-600 text-blue-600 hover:bg-blue-50 focus:ring-blue-500 active:scale-95",
        ghost: "text-blue-600 hover:bg-blue-50 focus:ring-blue-500 active:scale-95"
    };

    const sizes = {
        sm: "px-3 py-1.5 text-sm",
        md: "px-4 py-2 text-base",
        lg: "px-6 py-3 text-lg"
    };

    return (
        <button
            onClick={onClick}
            disabled={disabled || loading}
            className={`${baseStyles} ${variants[variant]} ${sizes[size]} ${className}`}
        >
            <RefreshCw
                className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`}
            />
            {loading ? 'Retrying...' : children}
        </button>
    );
};

export default RetryButton;