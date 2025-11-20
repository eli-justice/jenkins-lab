/**
 * @Author: Maclean Ayarik maclean@eganow.com
 * @Date: 2025-11-07 16:24:50
 * @LastEditors: Maclean Ayarik maclean@eganow.com
 * @LastEditTime: 2025-11-11 08:13:46
 * @FilePath: enLocale.ts
 * @Description: List of currency locale
 */
export interface EnLocaleCurrency {
    locale: string;
    country: string;
    currency: string;
}

export const enLocaleCurrencyList:EnLocaleCurrency[] = [
    { locale: "en-US", country: "United States", currency: "USD" },
    { locale: "en-GB", country: "United Kingdom", currency: "GBP" },
    { locale: "en-CA", country: "Canada", currency: "CAD" },
    { locale: "en-AU", country: "Australia", currency: "AUD" },
    { locale: "en-NZ", country: "New Zealand", currency: "NZD" },
    { locale: "en-IE", country: "Ireland", currency: "EUR" },
    { locale: "en-ZA", country: "South Africa", currency: "ZAR" },
    { locale: "en-NG", country: "Nigeria", currency: "NGN" },
    { locale: "en-GH", country: "Ghana", currency: "GHS" },
    { locale: "en-KE", country: "Kenya", currency: "KES" },
    { locale: "en-UG", country: "Uganda", currency: "UGX" },
    { locale: "en-TZ", country: "Tanzania", currency: "TZS" },
    { locale: "en-ZM", country: "Zambia", currency: "ZMW" },
    { locale: "en-BW", country: "Botswana", currency: "BWP" },
    { locale: "en-LR", country: "Liberia", currency: "LRD" },
    { locale: "en-SL", country: "Sierra Leone", currency: "SLL" },
    { locale: "en-GM", country: "Gambia", currency: "GMD" },
    { locale: "en-CM", country: "Cameroon", currency: "XAF" },
    { locale: "en-RW", country: "Rwanda", currency: "RWF" },
    { locale: "en-SZ", country: "Eswatini", currency: "SZL" },
    { locale: "en-LS", country: "Lesotho", currency: "LSL" },
    { locale: "en-MW", country: "Malawi", currency: "MWK" },
    { locale: "en-ZW", country: "Zimbabwe", currency: "ZWL" },
    { locale: "en-IN", country: "India", currency: "INR" },
    { locale: "en-SG", country: "Singapore", currency: "SGD" },
    { locale: "en-HK", country: "Hong Kong", currency: "HKD" },
    { locale: "en-PH", country: "Philippines", currency: "PHP" },
    { locale: "en-MY", country: "Malaysia", currency: "MYR" },
    { locale: "en-PK", country: "Pakistan", currency: "PKR" },
    { locale: "en-AE", country: "United Arab Emirates", currency: "AED" },
    { locale: "en-SA", country: "Saudi Arabia", currency: "SAR" },
    { locale: "en-QA", country: "Qatar", currency: "QAR" },
    { locale: "en-BB", country: "Barbados", currency: "BBD" },
    { locale: "en-TT", country: "Trinidad and Tobago", currency: "TTD" },
    { locale: "en-JM", country: "Jamaica", currency: "JMD" },
    { locale: "en-BS", country: "Bahamas", currency: "BSD" },
    { locale: "en-BZ", country: "Belize", currency: "BZD" },
    { locale: "en-GD", country: "Grenada", currency: "XCD" },
    { locale: "en-VC", country: "St. Vincent and the Grenadines", currency: "XCD" },
    { locale: "en-LC", country: "St. Lucia", currency: "XCD" },
    { locale: "en-KY", country: "Cayman Islands", currency: "KYD" },
    { locale: "en-BM", country: "Bermuda", currency: "BMD" },
    { locale: "en-FJ", country: "Fiji", currency: "FJD" },
    { locale: "en-PG", country: "Papua New Guinea", currency: "PGK" },
    { locale: "en-SB", country: "Solomon Islands", currency: "SBD" },
    { locale: "en-VU", country: "Vanuatu", currency: "VUV" }
];

/**
 * Get the currency for a given locale (e.g. "en-GH" → "GHS").
 */
export function getCurrency(locale: string): string | undefined {
    return enLocaleCurrencyList.find(
        (item) => item?.locale?.toLowerCase() === locale?.toLowerCase()
    )?.currency;
}

/**
 * Get the locale for a given country name (e.g. "Ghana" → "en-GH").
 */
export function getLocaleByCountry(country: string): string | undefined {
    return enLocaleCurrencyList.find(
        (item) => item?.country?.toLowerCase() === country?.toLowerCase()
    )?.locale;
}

/**
 * Get the locale for a given country name (e.g. "Ghana" → "en-GH").
 */
export function getLocaleByCurrency(currency: string): string | undefined {
    return enLocaleCurrencyList.find(
        (item) => item?.currency?.toLowerCase() === currency?.toLowerCase()
    )?.locale;
}