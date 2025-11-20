namespace Egapay.Customer.Business.Gateway.MerchantTransactions.Models;

public class NameEnquiryRequest
{
    public string Network { get; set; } = string.Empty;
    public string MobileOrAccountNumber { get; set; } = string.Empty;
    public string PaypartnerService { get; set; } = string.Empty;
}

public class InitiateCollectionPaymentRequest
{
    public string PaypartnerService { get; set; } = string.Empty;
    public string Paypartner { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string MobileOrAccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string Narration { get; set; } = string.Empty;
    public string CurrencyIso { get; set; } = string.Empty;
    public CardDetails? CardDetails { get; set; }
}

public class InitiatePayoutPaymentRequest : InitiateCollectionPaymentRequest
{
    
}

public class CardDetails
{
    public string Number { get; set; } = string.Empty;
    public string ExpiryMonth { get; set; } = string.Empty;
    public string ExpiryYear { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
}

public class PaymentChargesRequest
{
    public string PaypartnerService { get; set; } = string.Empty;
    public string Paypartner { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string MobileOrAccountNumber { get; set; } = string.Empty;
    public string CurrencyIso { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
}