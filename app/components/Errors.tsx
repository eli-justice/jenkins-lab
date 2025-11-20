/**
 * @Author: Maclean Ayarik maclean@eganow.com
 * @Date: 2025-11-07 08:32:30
 * @LastEditors: Maclean Ayarik maclean@eganow.com
 * @LastEditTime: 2025-11-11 10:23:48
 * @FilePath: app/components/Errors.tsx
 * @Description: A react page loader component
 */
import React from 'react';
import RetryButton from "@/app/components/RetryButton";

const Errors = () => {
    return (
        <div className="flex items-center justify-center h-screen w-screen bg-gradient-to-br from-red-200 to-sky-200">
            <div className="p-12 text-center">
                <div className="mb-6">
                    <svg className="w-24 h-24 text-red-500 mx-auto" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
                    </svg>
                </div>
                <h2 className="text-3xl font-bold text-gray-900 mb-3">Oops! Something went wrong</h2>
                <p className="text-gray-600 text-lg mb-4 max-w-md mx-auto">
                    We encountered an unexpected error. Don't worry, you can try refreshing the page or try later.
                </p>
                <div className="flex gap-4 justify-center">
                    <RetryButton
                        variant="outline"
                        size="lg"
                        onClick={() => window.location.reload()}
                        className="border-2 border-red-600  text-red-600 hover:bg-red-50 focus:ring-gray-200"
                    >
                        Retry
                    </RetryButton>
                </div>
            </div>
        </div>
    );
};

export default Errors;