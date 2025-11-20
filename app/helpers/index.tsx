/**
 * Formats a number as currency for a given locale and currency code.
 * @param amount - The number value to format.
 * @param locale - The locale string, e.g. "en-GB".
 * @param currency - The ISO currency code string, e.g. "GBP", "GHS".
 * @returns Formatted currency string.
 */

export function formatCurrency(
    amount: number,
    locale: string = "en-GB",
    currency: string = "GBP"
): string {
    return new Intl.NumberFormat(locale, {
        style: "currency",
        currency,
        currencyDisplay: "symbol",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }).format(amount);
}