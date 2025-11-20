import React from "react";
import egaLogo from "@/app/assets/images/eganow-logo.png";
import Image from "next/image";
import {formatCurrency} from "@/app/helpers";
import {getLocaleByCurrency} from "@/enLocale";
import {PaymentDetailsType} from "@/app/page";

type PropsHeaderType = {
    data: { currency: string; transAmount: number },
}


const Header: React.FC<PropsHeaderType> = ({ data }) => {
    const locale = data?.currency ? getLocaleByCurrency(data?.currency) : "en-GH";
    const formatted = data?.transAmount ? formatCurrency(data?.transAmount, locale, data?.currency) : null;

    return (
        <div className="mb-3">
            <div className="flex justify-between mb-3 gap-2 items-center">
                <Image src={egaLogo} alt="Eganow Logo" width={100} height={30} priority />
                <p className="font-bold text-green-600 bg-green-100 rounded-full px-5 py-1 border-green-200">
                    {formatted}
                </p>
            </div>
        </div>
    );
};

export default Header;
