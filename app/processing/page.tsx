"use client"
import React from 'react'
import {useSearchParams} from "next/navigation";
import {CheckCircle} from "lucide-react";


function page() {
    const searchParams = useSearchParams();

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-red-100 to-blue-100 p-4">

        <div className="bg-gradient-to-r p-8 text-center relative overflow-hidden rounded">
            <div className="relative z-10">
                {/* Animated loader icon */}
                <div className="inline-flex items-center justify-center mb-4 relative">
                    <CheckCircle className="w-50 h-50 text-green-500 text-cente"/>
                </div>

                <h1 className="text-3xl font-bold text-green-500 mb-2">
                    Transaction Initiated
                </h1>
                <p className="text-gray-800 text-lg">
                    Your transaction has been successfully initiated and is now being processed.
                </p>
                <div className="bg-gray-100 rounded-lg p-4 font-mono text-base break-all text-gray-800 border border-gray-300">
                   REF. NO::  {searchParams.get("ref")}
                </div>
            </div>
        </div>
    </div>
  )
}

export default page