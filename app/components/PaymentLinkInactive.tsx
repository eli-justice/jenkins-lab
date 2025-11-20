/**
 * @Author: Maclean Ayarik maclean@eganow.com
 * @Date: 2025-11-07 08:32:30
 * @LastEditors: Maclean Ayarik maclean@eganow.com
 * @LastEditTime: 2025-11-11 08:01:56
 * @FilePath: app/components/PaymentLinkInactive.tsx
 * @Description: A react page loader component
 */
'use client'
import React from 'react';
import { AlertCircle, Lock, Calendar, ExternalLink, Home } from 'lucide-react';

const PaymentLinkInactive = () => {
    return (
        <div className="min-h-screen bg-gradient-to-br from-red-200 via-gray-100 to-sky-200 flex items-center justify-center p-4">
            <div className="max-w-2xl w-full">

                {/* Main Card */}
                <div className="bg-white rounded shadow-2xl overflow-hidden">

                    {/* Header with Icon */}
                    <div className="bg-gradient-to-r from-red-500 to-red-600 p-8 text-center">
                        <div className="inline-flex items-center justify-center w-20 h-20 bg-white rounded-full mb-4">
                            <Lock className="w-10 h-10 text-red-500" />
                        </div>
                        <h1 className="text-3xl font-bold text-white mb-2">Payment Link Not Active</h1>
                        <p className="text-red-100 text-lg">This payment link is currently unavailable</p>
                    </div>

                    {/* Content */}
                    <div className="p-8">

                        {/* Possible Reasons */}
                        <div className="mb-8">
                            <h3 className="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                                Possible Reasons
                            </h3>
                            <div className="space-y-3">
                                <div className="flex items-start gap-3 p-4 bg-gray-50 rounded-lg">
                                    <div className="w-6 h-6 rounded-full bg-red-100 flex items-center justify-center flex-shrink-0 mt-0.5">
                                        <span className="text-red-600 font-bold text-sm">1</span>
                                    </div>
                                    <div>
                                        <p className="font-semibold text-gray-900">Link Has Expired</p>
                                        <p className="text-gray-600 text-sm mt-1">Payment links have a validity period and this one may have expired.</p>
                                    </div>
                                </div>

                                <div className="flex items-start gap-3 p-4 bg-gray-50 rounded-lg">
                                    <div className="w-6 h-6 rounded-full bg-red-100 flex items-center justify-center flex-shrink-0 mt-0.5">
                                        <span className="text-red-600 font-bold text-sm">2</span>
                                    </div>
                                    <div>
                                        <p className="font-semibold text-gray-900">Payment Already Completed</p>
                                        <p className="text-gray-600 text-sm mt-1">This link was for a one-time payment that has already been processed.</p>
                                    </div>
                                </div>

                                <div className="flex items-start gap-3 p-4 bg-gray-50 rounded-lg">
                                    <div className="w-6 h-6 rounded-full bg-red-100 flex items-center justify-center flex-shrink-0 mt-0.5">
                                        <span className="text-red-600 font-bold text-sm">3</span>
                                    </div>
                                    <div>
                                        <p className="font-semibold text-gray-900">Link Was Cancelled</p>
                                        <p className="text-gray-600 text-sm mt-1">The merchant may have cancelled or deactivated this payment link.</p>
                                    </div>
                                </div>

                                <div className="flex items-start gap-3 p-4 bg-gray-50 rounded-lg">
                                    <div className="w-6 h-6 rounded-full bg-red-100 flex items-center justify-center flex-shrink-0 mt-0.5">
                                        <span className="text-red-600 font-bold text-sm">4</span>
                                    </div>
                                    <div>
                                        <p className="font-semibold text-gray-900">Invalid or Incorrect Link</p>
                                        <p className="text-gray-600 text-sm mt-1">The link may be incorrect or incomplete. Please verify the URL.</p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {/* What to Do Next */}
                        <div className="mb-8">
                            <h3 className="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                                What You Can Do
                            </h3>
                            <div className="bg-blue-50 border border-blue-200 rounded-lg p-6">
                                <ul className="space-y-3">
                                    <li className="flex items-start gap-3">
                                        <div className="w-5 h-5 rounded-full bg-blue-500 flex items-center justify-center flex-shrink-0 mt-0.5">
                                            <svg className="w-3 h-3 text-white" fill="currentColor" viewBox="0 0 20 20">
                                                <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                            </svg>
                                        </div>
                                        <p className="text-gray-700">Contact the merchant or service provider who sent you this link</p>
                                    </li>
                                    <li className="flex items-start gap-3">
                                        <div className="w-5 h-5 rounded-full bg-blue-500 flex items-center justify-center flex-shrink-0 mt-0.5">
                                            <svg className="w-3 h-3 text-white" fill="currentColor" viewBox="0 0 20 20">
                                                <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                            </svg>
                                        </div>
                                        <p className="text-gray-700">Request a new payment link if the original has expired</p>
                                    </li>
                                    <li className="flex items-start gap-3">
                                        <div className="w-5 h-5 rounded-full bg-blue-500 flex items-center justify-center flex-shrink-0 mt-0.5">
                                            <svg className="w-3 h-3 text-white" fill="currentColor" viewBox="0 0 20 20">
                                                <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                            </svg>
                                        </div>
                                        <p className="text-gray-700">Double-check that you have the correct and complete payment link</p>
                                    </li>
                                    <li className="flex items-start gap-3">
                                        <div className="w-5 h-5 rounded-full bg-blue-500 flex items-center justify-center flex-shrink-0 mt-0.5">
                                            <svg className="w-3 h-3 text-white" fill="currentColor" viewBox="0 0 20 20">
                                                <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                            </svg>
                                        </div>
                                        <p className="text-gray-700">Check your email for any updates or alternative payment instructions</p>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Additional Help Text */}
                <div className="mt-6 text-center">
                    <p className="text-gray-600 text-sm">
                        If you believe this is an error, please contact the merchant directly or reach out to our support team.
                    </p>
                </div>

            </div>
        </div>
    );
};

export default PaymentLinkInactive;