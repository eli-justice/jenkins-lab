namespace Egapay.Customer.Business.Gateway.Common.Utils.Routes;

public static class MerchantTransactionRoutes
{
    public const string DashboardEndpoint = "transactions/dashboard";
    public const string AccountTransactionEndpoint = "transactions/account";
    public const string FundMerchantWalletEndpoint = "transactions/fund-wallet/request";
    public const string GetFundMerchantWalletEndpoint = "transactions/fund-wallet";
    public const string GetPaypartnersEndpoint = "payments/paypartners";
    public const string GetNameEnquiryEndpoint = "payments/name-enquiry";
    public const string InitiateCollectionEndpoint = "payments/collection";
    public const string InitiatePayoutEndpoint = "payments/payout";
    public const string AccountBalancesEndpoint = "merchants/balances";
    public const string PaymentChargesEndpoint = "payments/charges";
    
    public const string USSDMerchantCustomersUrlEndpoint = "merchants/ussd-customers";
    public const string ApproveMerchantCustomerUrlEndpoint = $"{USSDMerchantCustomersUrlEndpoint}/review";

    public const string USSDMerchantCustomerPinResetUrlEndpoint = $"{USSDMerchantCustomersUrlEndpoint}/pin-reset-requests";
    public const string ApproveMerchantCustomerPinResetUrlEndpoint = $"{USSDMerchantCustomerPinResetUrlEndpoint}/review";
}