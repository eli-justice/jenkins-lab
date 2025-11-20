import React from "react";

function Footer() {
    return (
        <div className="mt-1 flex justify-center items-center text-sm text-gray-500 p-2">
            <img src="secure-payment-lock.svg" alt="Secured Payment" className="h-5 w-5 mr-1"/>
            Secured by <span className="font-medium ml-1 text-blue-500">Eganow</span>
        </div>
    );
}

export default Footer;
