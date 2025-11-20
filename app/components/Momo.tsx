"use client";

import React from "react";
import mtn from "@/app/assets/images/mtn.png";
import at from "@/app/assets/images/at.png";
import telecel from "@/app/assets/images/telecel.png";
import NetworkSelect from "./NetworkSelect";
import {Controller, useForm} from "react-hook-form";
import {getKYC, getPartners, makePayment} from "@/api/checkoutAPI";
import {MerchantResponseType, PaymentDetailsType} from "@/app/page";
import {useQuery} from "@tanstack/react-query";
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

const TELCO_IMAGES = {
    MTNGH: mtn,
    ATGH: at,
    TCELGH: telecel,
}

type TelcoCode = keyof typeof TELCO_IMAGES;

function defaultFormValues(data: PaymentDetailsType) {
    return {
        "paypartnerCode": "",
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


function Momo(props: PropTypes) {
    const router = useRouter();
    const defaultValues = React.useMemo(
        () => (defaultFormValues(props?.data?.paymentDetails)), [props?.data]);
    const [networkOptions, setNetworkOptions] = React.useState([])

    const {
        register,
        handleSubmit,
        formState: {errors, isSubmitting},
        control,
        watch,
        setValue,
    } = useForm({
        mode: "onChange",
        defaultValues,
    });

    const watchPaypartnerCode = watch("paypartnerCode")
    const watchMobileNumber = watch("accountNoOrCardNoOrMSISDN")

    const {
        data: pay_partners_data,
        isFetching
    } = useQuery({
        queryKey: ["eganow-checkout-pay-partners", {}],
        queryFn: () =>
            getPartners({
                "countryCode": props?.data?.filter?.CountryCode,
                "languageId": "en"
            }, props?.data?.paymentDetails?.xauth, props?.data?.developerJwtToken),
        enabled: true,
        refetchOnWindowFocus: false,
        staleTime: 5000,
    });

    const {
        data: kyc_data,
        isFetching: kycIsFetching,
        refetch: kycRefetch,
    } = useQuery({
        queryKey: ["eganow-checkout-kyc", watchPaypartnerCode],
        queryFn: () =>
            getKYC({
                "paypartnerCode": watchPaypartnerCode,
                "accountNoOrCardNoOrMSISDN": watchMobileNumber,
                "countryCode": props?.data?.filter?.CountryCode,
                "languageId": "en",
            }, props?.data?.paymentDetails?.xauth, props?.data?.developerJwtToken),
        enabled: false,
    });

    React.useEffect(() => {
        const code = (watchPaypartnerCode ?? "").toString().trim();
        const mobileNum = String(watchMobileNumber);
        // Trigger refetch when paypartnerCode is not empty and mobile number >= 10
        if (code.length > 0 && mobileNum.length >= 10) {
            // Optional small debounce to avoid rapid consecutive fetches
            const t = setTimeout(() => {
                kycRefetch();
            }, 200); // adjust as needed
            return () => clearTimeout(t);
        }
    }, [watchPaypartnerCode, watchMobileNumber, kycRefetch]);

    React.useEffect(() => {
        if (!!kyc_data?.data?.accountName) setValue("accountName", kyc_data?.data?.accountName, {shouldValidate: true});
    }, [kyc_data]);

    React.useEffect(() => {
        // Early return if no data
        if (!pay_partners_data?.data) {
            setNetworkOptions([]);
            return;
        }
        const options = pay_partners_data.data
            .filter((obj: { paypartnerCode: string; transType: string }) =>
                obj.transType === "MOMO"
            )
            .map((obj: { paypartnerCode: string; transType: string }) => ({
                value: obj.paypartnerCode,
                label: obj.paypartnerCode,
                image: TELCO_IMAGES[obj.paypartnerCode as TelcoCode] || '',
            }));
        setNetworkOptions(options);
    }, [pay_partners_data?.data]);

    const onSubmit = async (data: any) => {
        try {
            const transactionId = generateUUID();
            // Split expiry date into month and year
            const payload = {
                ...data,
                "transactionId": transactionId,
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
            <p className="text-center text-sm text-gray-400">
                Enter your Mobile money details to pay
            </p>

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                {/* Network */}
                <div>
                    <label className="block text-sm font-medium mb-1">
                        Network
                    </label>
                    <Controller
                        name="paypartnerCode"
                        control={control}
                        rules={{required: "Network is required"}}
                        render={({field, fieldState}) => {
                            // Map the current field value to the corresponding option object
                            const selected = networkOptions.find((opt:{value:string}) => opt.value === field.value) ?? null;

                            return (
                                <NetworkSelect
                                    id="paypartnerCode"
                                    options={networkOptions}
                                    value={selected}
                                    isLoading={isFetching}
                                    onChange={(opt:{value:string}) => {
                                        field.onChange(opt?.value ?? "");
                                    }}
                                    error={Boolean(fieldState.error)}
                                />
                            );
                        }}
                    />
                    {errors.paypartnerCode && (
                        <p className="text-red-500 text-xs">
                            {String(errors.paypartnerCode.message)}
                        </p>
                    )}
                </div>

                {/* Phone Number */}
                <div>
                    <label className="block text-sm font-medium text-gray-900 mb-1">
                        Phone Number
                    </label>
                    <input
                        type="tel"
                        placeholder="e.g. 024XXXXXXX"
                        {...register("accountNoOrCardNoOrMSISDN", {
                            required: "Phone number is required",
                            pattern: {
                                value: /^[0-9]{10}$/,
                                message: "Enter a valid 10-digit number",
                            },
                        })}
                        className={`w-full text-gray-600 px-2 py-2 text-sm border ${
                            errors.accountNoOrCardNoOrMSISDN ? "border-red-500" : "border-gray-300"
                        } rounded-md focus:outline-none focus:ring-0`}
                    />
                    {errors.accountNoOrCardNoOrMSISDN && (
                        <p className="text-red-500 text-xs">
                            {String(errors.accountNoOrCardNoOrMSISDN.message)}
                        </p>
                    )}
                </div>

                {/* Account Name */}
                <div>
                    <label className="block text-sm font-medium text-gray-900 mb-1">
                        Account Name
                    </label>
                    <div style={{position: "relative"}}>
                        <input
                            id="accountName"
                            type="text"
                            placeholder="Enter account name"
                            {...register("accountName", {required: "Account name is required"})}
                            className={`w-full text-gray-600 px-2 py-2 text-sm border ${
                                errors.accountName ? "border-red-500" : "border-gray-300"
                            } rounded-md focus:outline-none focus:ring-0 ${kycIsFetching ? "opacity-70" : ""}`}
                            aria-invalid={!!errors.accountName}
                            aria-describedby={errors.accountName ? "accountName-error" : undefined}
                        />
                        {kycIsFetching && (
                            <span
                                aria-label="loading"
                                role="status"
                                className="absolute right-2 top-1/2 transform -translate-y-1/2 text-xs text-gray-500"
                            >
                                Loading kyc…
                              </span>
                        )}
                    </div>
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
                    className="w-full mt-1 bg-red-600 text-white py-3 rounded-xl font-semibold hover:opacity-90 transition duration-200 cursor-pointer hover:bg-red-700  hover:shadow-xl hover:shadow-red-200 "
                >
                    {isSubmitting ? "Processing..." : "Pay Now"}
                </button>
            </form>
        </div>
    );
}

export default Momo;
