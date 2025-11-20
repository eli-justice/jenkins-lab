namespace Egapay.Customer.Business.Gateway.Common.Utils.Routes;

public static class MerchantOnboardingRoutes
{
    public const string LoginEndpoint = "auth/login";
    public const string RequestPasswordResetEndpoint = "auth/request-password-reset";
    public const string PasswordResetEndpoint = "auth/password-reset";
    
    //common
    public const string CountryEndpoint = "countries";
    public const string GenerateOtpEndpoint = "otp/generate";
    public const string VerifyOtpEndpoint = "otp/verify";
    
    //merchant
    public const string CheckMerchantAccountEndpoint = "merchants/check-account";
    public const string RegisterMerchantEndpoint = "merchants/register";
    public const string MerchantBusinessInfoEndpoint = "merchants/accounts/info";
    public const string ActiveRegulatorsEndpoint = "merchants/active-regulators";
    public const string ActiveIndustriesEndpoint = "merchants/active-industries";
    public const string MerchantContactInfoEndpoint = "merchants/accounts/contact-info";
    public const string MerchantContactPersonEndpoint = "merchants/accounts/contact-persons";
    public const string MerchantDirectorEndpoint = "merchants/accounts/director-shareholders";
    public const string MerchantDocumentEndpoint = "merchants/accounts/documents";
    public const string MerchantServicesEndpoint = "merchants/services";
    public const string MerchantProfileEndpoint = "merchants/profile";
    public const string MerchantApiKeysEndpoint = "merchants/api-keys";
    public const string MerchantCallbackUrlEndpoint = "merchants/callback-urls";
    
    //health checks
    public const string HealthChecksEndpoint = "health-checks";
}