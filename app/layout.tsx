import type {Metadata} from "next";
import {Geist, Geist_Mono} from "next/font/google";
import "./globals.css";
import TanstackQueryClientProvider from "@/app/providers/TanstackQueryClientProvider";
import {Suspense} from "react";
import PageLoader from "@/app/components/PageLoader";

const geistSans = Geist({
    variable: "--font-geist-sans",
    subsets: ["latin"],
});

const geistMono = Geist_Mono({
    variable: "--font-geist-mono",
    subsets: ["latin"],
});

export const metadata: Metadata = {
    title: "Eganow Checkout",
    description: "Choose a payment method to complete your checkout.",
};

export default function RootLayout({children}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html lang="en">
            <body className={`${geistSans.variable} ${geistMono.variable} antialiased`} cz-shortcut-listen="true">
            <Suspense fallback={<PageLoader/>}>
                <TanstackQueryClientProvider>{children}</TanstackQueryClientProvider>
            </Suspense>
            </body>
        </html>
    );
}
