import axiosInstance from "../axios-instance";

const basicAuth = 'Basic ' + btoa(`${process.env.NEXT_PUBLIC_API_USERNAME}:${process.env.NEXT_PUBLIC_API_PASSWORD}`);

export const getMerchantDetails = async (payload: object) => {
    return  await axiosInstance.post("/api/hosted-checkout/get-merchant-details", payload,{
        headers: {
            "Authorization": basicAuth,
        },
    });
};

export const makePayment = async (payload: object, xAuth:string, token:string) => {
    return await axiosInstance.post("api/transactions/collection", payload, {
        headers: {
            "Authorization": "Bearer " + token,
            "x-Auth" : xAuth,
        },
    });
};

export const getPartners = async (payload: object, xAuth:string, token:string) => {
    return await axiosInstance.post("api/partners/search", payload, {
        headers: {
            "Authorization": "Bearer " + token,
            "x-Auth" : xAuth,
        },
    });
};

export const getKYC = async (payload: object, xAuth:string, token:string) => {
    return await axiosInstance.post("api/vas/kyc", payload, {
        headers: {
            "Authorization": "Bearer " + token,
            "x-Auth" : xAuth,
        },
    });
};


