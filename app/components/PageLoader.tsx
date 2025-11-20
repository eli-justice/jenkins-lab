/**
 * @Author: Maclean Ayarik maclean@eganow.com
 * @Date: 2025-11-07 08:32:30
 * @LastEditors: Maclean Ayarik maclean@eganow.com
 * @LastEditTime: 2025-11-11 08:00:31
 * @FilePath: app/components/PageLoader.tsx
 * @Description: A react page loader component
 */
'use client'

import React from 'react';

const PageLoader = () => {
    return (
        <div className="flex items-center justify-center h-screen w-screen bg-gradient-to-br from-red-200 to-sky-200">
            {/* Container for the circles */}
            <div className="flex space-x-2">
                {/* Circle 1: Red */}
                <div
                    className="w-10 h-10 bg-red-500 rounded-full animate-custom-blink"
                    style={{animationDelay: '-0.32s'}}
                />

                {/* Circle 2: Blue */}
                <div
                    className="w-10 h-10 bg-yellow-500 rounded-full animate-custom-blink"
                    style={{animationDelay: '-0.16s'}}
                />

                {/* Circle 3: Green */}
                <div
                    className="w-10 h-10 bg-blue-500 rounded-full animate-custom-blink"
                    style={{animationDelay: '0s'}}
                />
            </div>
        </div>
    );
};

export default PageLoader;