"use client";

import React from "react";
import Header from "./components/Header";
import Card from "./components/Card";
import Momo from "./components/Momo";
import PageLoader from "./components/PageLoader";
import PaymentLinkInactive from "./components/PaymentLinkInactive";
import Errors from "./components/Errors";
import ItemList from "./components/ItemList";
import Footer from "./components/Footer";
import Each from "@/app/components/Each";
import {useQuery} from "@tanstack/react-query";
import {getMerchantDetails} from "@/api/checkoutAPI";
import {useSearchParams} from 'next/navigation'
import "react-credit-cards-2/dist/lib/styles.scss";

export interface PaymentDetailsType {
    merchantServiceId: string;
    currency: string;
    transAmount: number;
    canuserchangeamountyesno: "YES" | "NO";
    singleOrMultipleUse: "SINGLE_USE" | "MULTIPLE_USE";
    narration: string;
    paymentLinkStatus: "ACTIVE" | "INACTIVE" | string;
    xauth: string;
    items?: Array<{ id: string; name: string; amount: number; }>;
}

export interface MerchantResponseType {
    isSuccess: boolean;
    message: string;
    developerJwtToken: string;
    paymentDetails: PaymentDetailsType;
    filter:{ TransRefNumber: string; CountryCode: string }
}

const PAYMENT_METHODS = [
    {name: "Card", icon: "credit-card-icon.svg"},
    {name: "Mobile money", icon: "mobile-money-icon.svg"}
];

function Page() {
    const searchParams = useSearchParams()
    const [selectedMethod, setSelectedMethod] = React.useState<string>("Card");
    const [headerData, setHeaderData] = React.useState<{ currency: string; transAmount: number }>({
        currency:  "",
        transAmount: 0,
    });

    const [filter, setFilter] = React.useState<{ TransRefNumber: string; CountryCode: string }>({
        TransRefNumber: searchParams.get('p') || "",
        CountryCode: searchParams.get('c') || "",
    });

    React.useLayoutEffect(() => {
        setFilter({
            TransRefNumber: searchParams.get('p') || "",
            CountryCode: searchParams.get('c') || "",
        });
    }, [searchParams]);

    const {
        data: merchant_details_data,
        isError,
        isFetching,
        isLoading,
    } = useQuery({
        queryKey: ["eganow-checkout", {}],
        queryFn: () =>
            getMerchantDetails(filter),
        enabled: !!filter.TransRefNumber && !!filter.CountryCode,
        refetchOnWindowFocus: false,
        staleTime: 5000,
    });

    React.useLayoutEffect(()=> {
        setHeaderData({
            currency: merchant_details_data?.data?.paymentDetails?.currency,
            transAmount: merchant_details_data?.data?.paymentDetails?.transAmount,
        })
    },[merchant_details_data?.data?.paymentDetails])

    // Memoize computed values
    const paymentDetails = React.useMemo(
        () => merchant_details_data?.data?.paymentDetails,
        [merchant_details_data]
    )

    const hasItems = React.useMemo(
        () => Array.isArray(paymentDetails?.items) && paymentDetails?.items?.length > 0,
        [paymentDetails]
    );

    // Conditional UI States
    if (isFetching || isLoading) return <PageLoader/>;

    if ((isError || !paymentDetails) && !isFetching) return <Errors/>;

    if ((paymentDetails?.paymentLinkStatus !== "ACTIVE") && (typeof paymentDetails?.paymentLinkStatus !== "undefined")) {
        return <PaymentLinkInactive/>;
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-red-200 to-sky-200">
            <div
                className="backdrop-blur-xl bg-gradient-to-br from-white/20 to-white/5 border border-white/30 shadow-2xl p-3 rounded-3xl">
                <div className="flex gap-5">

                    {/* ITEM LIST SECTION */}
                    {hasItems && (
                        <section className="flex-1 p-5 bg-gray-50 min-w-[300px] shadow-xl rounded-3xl">
                            <ItemList data={paymentDetails}/>
                        </section>
                    )}

                    {/* CHECKOUT SECTION */}
                    <section className="flex-1 px-5 py-5 bg-white min-w-[350px] w-[300px] shadow-xl rounded-2xl">
                        <Header data={headerData}/>
                        {/* CARD / MOMO */}
                        <div className="grid grid-cols-2 gap-x-3 mb-5">
                            <Each of={PAYMENT_METHODS} render={(option) => (
                                <button
                                    key={option.name}
                                    type="button"
                                    onClick={() => setSelectedMethod(option.name)}
                                    className={`py-2 text-xs rounded-md flex items-center gap-4 justify-center transition-colors duration-200 
                                    ${
                                        selectedMethod === option.name
                                            ? "bg-green-50 border-2 border-green-400 text-green-400 font-semibold"
                                            : "bg-gray-50 text-gray-700 border border-gray-200"
                                    }`}
                                >
                                    <img src={option.icon} alt="Secured Payment" className="h-7 w-7 text-green-400"/>
                                    {option.name}
                                </button>
                            )}/>
                        </div>
                        {
                            selectedMethod == 'Card' ?
                                <Card data={{...merchant_details_data?.data, filter}} setHeaderData = {setHeaderData}/>
                                :
                                <Momo data={{...merchant_details_data?.data, filter}} setHeaderData = {setHeaderData}/>
                        }
                        <Footer/>
                    </section>
                </div>
            </div>
        </div>
    );
}

export default Page;
