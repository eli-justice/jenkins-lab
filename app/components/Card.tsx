"use client";

import React from "react";
import {useForm} from "react-hook-form";
import Cards from "react-credit-cards-2";
import {makePayment} from "@/api/checkoutAPI";
import {MerchantResponseType, PaymentDetailsType} from "@/app/page";
import {formatCurrency} from "@/app/helpers";
import {getLocaleByCurrency} from "@/enLocale";
import {generateUUID} from "@/lib/generateUUID";
import {useRouter} from "next/navigation";

interface HeaderData {
    currency: string;
    transAmount: number;
}

type PropTypes = {
    data: MerchantResponseType,
    setHeaderData: React.Dispatch<React.SetStateAction<HeaderData>>;
}


function defaultFormValues(data: PaymentDetailsType) {
    return {
        "paypartnerCode": "CARDGATEWAY",
        "amount": data?.transAmount,
        "accountNoOrCardNoOrMSISDN": "",
        "accountName": "",
        "transactionId": "",
        "narration": data?.narration,
        "transCurrencyIso": data?.currency,
        "expiryDateMonth": 0,
        "expiryDateYear": 0,
        "expiryDate": "",
        "cvv": "",
        "languageId": "en",
        "callback": "",
    };
}

function Card(props: PropTypes) {
    const router = useRouter();
    const defaultValues = React.useMemo(
        () => (defaultFormValues(props?.data?.paymentDetails)), [props?.data]);

    const {
        register,
        handleSubmit,
        setValue,
        watch,
        formState: {errors, isSubmitting},
    } = useForm({
        mode: "onChange",
        defaultValues,
    });

    const watchCardNo = watch('accountNoOrCardNoOrMSISDN');
    const watchAccountName = watch('accountName');
    const watchCardCVV = watch('cvv');
    const watchExpiryDate = watch('expiryDate');

    // Automatically format card number: 1234 5678 9012 3456
    const handleCardNumberChange = (e: any) => {
        let value = e.target.value.replace(/\D/g, ""); // remove non-digits
        if (value.length > 16) value = value.slice(0, 16); // limit to 16 digits
        const formattedValue = value.replace(/(.{4})/g, "$1 ").trim(); // add space every 4 digits
        setValue("accountNoOrCardNoOrMSISDN", formattedValue, {shouldValidate: true});
    };

    // Automatically format expiry date: 12 → 12/, 1226 → 12/26
    const handleExpiryDateChange = (e: any) => {
        let value = e.target.value.replace(/\D/g, ""); // remove non-digits
        if (value.length > 4) value = value.slice(0, 4);

        // Auto add slash after month (MM)
        if (value.length > 2) {
            value = value.slice(0, 2) + "/" + value.slice(2);
        }
        setValue("expiryDate", value, {shouldValidate: true});
    };

    const onSubmit = async (data: any) => {
        try {
            const transactionId = generateUUID();
            // Split expiry date into month and year
            const [month, year] = data.expiryDate.split("/");
            const payload = {
                ...data,
                "transactionId": transactionId,
                "accountNoOrCardNoOrMSISDN": data.accountNoOrCardNoOrMSISDN.replace(/\s/g, ""),
                "expiryDateMonth": parseInt(month),
                "expiryDateYear": parseInt(year),
            };

            const response: any = await makePayment(payload, props?.data?.paymentDetails?.xauth, props?.data?.developerJwtToken);
            //On success of endpoint
            if (response?.data?.transactionStatus === "Pending") {
                //Showing feedback message
                router.push(`/processing?ref=${response?.data?.eganowReferenceNo}`);
            } else {
                throw new Error(response?.message);
            }
            // router.push(`/processing`);
        } catch (error) {
            console.error("Error:", error);
        }
    };

    return (
        <div>
            <div>
                <p className="text-center text-sm text-gray-400">
                    Enter your card details to pay
                </p>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    {/* Card Image */}
                    <div className="py-2">
                        <Cards
                            number={watchCardNo}
                            expiry={watchExpiryDate}
                            cvc={watchCardCVV}
                            name={watchAccountName}
                        />
                    </div>

                    {/*  Card Number */}
                    <div>
                        <label className="block text-sm font-medium text-gray-900 mb-1">
                            Card Number
                        </label>
                        {/* card n */}
                        <div className="flex relative">
                            <input
                                type="text"
                                placeholder="0000 0000 0000 0000"
                                {...register("accountNoOrCardNoOrMSISDN", {
                                    required: "Card number is required",
                                    validate: (val) =>
                                        val.replace(/\s/g, "").length === 16 ||
                                        "Enter a valid 16-digit card number",
                                })}
                                onChange={handleCardNumberChange}
                                maxLength={19}
                                className={`w-full text-gray-600 px-2 py-2 text-sm border rounded ${
                                    errors.accountNoOrCardNoOrMSISDN ? "border-red-500" : "border-gray-300"
                                }  focus:outline-none focus:ring-0 focus:ring-none`}
                            />

                            <div className="card absolute -top-[72px] -right-[120px] ">
                                <Cards
                                    number={watchCardNo}
                                    expiry={watchExpiryDate}
                                    cvc={watchCardCVV}
                                    name={watchAccountName}
                                />
                            </div>
                        </div>
                        {errors.accountNoOrCardNoOrMSISDN && (
                            <p className="text-xs text-red-500 mt-1">
                                {errors.accountNoOrCardNoOrMSISDN.message}
                            </p>
                        )}
                    </div>

                    {/* Expiry and CVV */}
                    <div className="flex gap-1">
                        <div className="w-1/2">
                            <label className="block text-sm font-medium text-gray-900 mb-1">
                                Expiry Date
                            </label>
                            <input
                                type="text"
                                placeholder="MM/YY"
                                {...register("expiryDate", {
                                    required: "Expiry date is required",
                                    pattern: {
                                        value: /^(0[1-9]|1[0-2])\/\d{2}$/,
                                        message: "Invalid format (MM/YY)",
                                    },
                                })}
                                onChange={handleExpiryDateChange}
                                maxLength={5}
                                className={`w-full text-gray-600 px-2 py-2 text-sm border rounded ${
                                    errors.expiryDate ? "border-red-500" : "border-gray-300"
                                }  focus:outline-none focus:ring-0 focus:ring-blue-400`}
                            />
                            {errors.expiryDate && (
                                <p className="text-xs text-red-500 mt-1">
                                    {errors.expiryDate.message}
                                </p>
                            )}
                        </div>

                        <div className="w-1/2">
                            <label className="block text-sm font-medium text-gray-900 mb-1">
                                CVC
                            </label>
                            <input
                                type="text"
                                maxLength={3}
                                placeholder="CVV"
                                {...register("cvv", {
                                    required: "CVV is required",
                                    pattern: {
                                        value: /^[0-9]{3}$/,
                                        message: "Invalid CVV",
                                    },
                                })}
                                className={`w-full text-gray-600 px-2 py-2 text-sm border rounded ${
                                    errors.cvv ? "border-red-500" : "border-gray-300"
                                } focus:outline-none focus:ring-0 focus:ring-blue-400`}
                            />
                            {errors.cvv && (
                                <p className="text-xs text-red-500 mt-1">
                                    {errors.cvv.message}
                                </p>
                            )}
                        </div>
                    </div>

                    {/* Cardholder Name */}
                    <div>
                        <label className="block text-sm font-medium text-gray-900 mb-1">
                            Name on card
                        </label>
                        <input
                            type="text"
                            placeholder="John Doe"
                            {...register("accountName", {
                                required: "Name on card is required",
                            })}
                            className={`w-full text-gray-600 px-2 py-2 text-sm border ${
                                errors.accountName ? "border-red-500" : "border-gray-300"
                            } rounded-md focus:outline-none focus:ring-0 focus:ring-blue-400`}
                        />
                        {errors.accountName && (
                            <p className="text-xs text-red-500 mt-1">
                                {String(errors.accountName.message)}
                            </p>
                        )}
                    </div>

                    {/* Amount */}
                    {props.data?.paymentDetails?.canuserchangeamountyesno === "YES" &&
                        <div>
                            <label className="block text-sm font-medium text-gray-900 mb-1">
                                Amount
                            </label>
                            <input
                                type="number"
                                placeholder="Enter amount here"
                                {...register("amount", {
                                    required: "Amount is required",
                                })}
                                onChange={(e) => {
                                    const value = e.target.value || 0;
                                    // @ts-ignore
                                    props.setHeaderData((prev:{ currency: string; transAmount: number }) => ({
                                        ...prev,
                                        transAmount: value,
                                    }));
                                }}
                                className={`w-full text-gray-600 p-2 text-sm border ${
                                    errors.amount ? "border-red-500" : "border-gray-300"
                                } rounded-md focus:outline-none focus:ring-0`}
                            />
                            {errors.amount && (
                                <p className="text-xs text-red-500 mt-1">
                                    {String(errors.amount.message)}
                                </p>
                            )}
                        </div>
                    }

                    {/*Narration Name */}
                    <div>
                        <label className="block text-sm font-medium text-gray-900 mb-1">
                            Narration
                        </label>
                        <textarea
                            maxLength={255}
                            rows={2}
                            placeholder="Enter narration here"
                            {...register("narration", {
                                required: "Narration is required",
                            })}
                            className={`w-full text-gray-600 px-4 py-2 border ${
                                errors.narration ? "border-red-500" : "border-gray-300"
                            } rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400`}
                        />
                        {errors?.narration && (
                            <p className="text-xs text-red-500 mt-1">
                                {String(errors?.narration?.message)}
                            </p>
                        )}
                    </div>

                    {/* Pay Button */}
                    <button
                        type="submit"
                        disabled={isSubmitting}
                        className="w-full bg-red-600 text-white py-3 rounded-xl font-semibold hover:opacity-90 transition duration-200 cursor-pointer hover:bg-red-700  hover:shadow-xl hover:shadow-red-200 "
                    >
                        {isSubmitting ? "Processing..." : "Pay " + formatCurrency(props?.data?.paymentDetails?.transAmount, getLocaleByCurrency(props?.data?.paymentDetails?.currency), props?.data?.paymentDetails?.currency)}
                    </button>
                </form>
            </div>
        </div>
    );
}

export default Card;
