import React from "react";
import { formatCurrency } from "@/app/helpers";
import { getLocaleByCurrency } from "@/enLocale";
import { PaymentDetailsType } from "@/app/page";

const ItemList = React.memo(({ data }: { data: PaymentDetailsType }) => {
    return (
        <section className="text-gray-500" aria-label="Order Summary">
            <h2 className="text-xl font-semibold mb-4 text-start">Order Summary</h2>

            <div className="divide-y">
                {data?.items?.map((item) => (
                    <div key={item.id} className="flex justify-between py-3 text-sm">
                        <span>{item.name}</span>
                        <span className="font-bold">
                            {formatCurrency(item.amount, getLocaleByCurrency(data?.currency), data?.currency)}
                        </span>
                    </div>
                ))}
            </div>

            <div className="flex justify-between mt-4 pt-4 border-t font-semibold text-lg" aria-label="Total">
                <span>Total</span>
                <span className="font-bold">
                    {formatCurrency(data?.transAmount, getLocaleByCurrency(data?.currency), data?.currency)}
                </span>
            </div>
        </section>
    );
});

export default ItemList;
