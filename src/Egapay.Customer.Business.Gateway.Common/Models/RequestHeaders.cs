using Egapay.Customer.Business.Gateway.Common.Utils.Enums;

namespace Egapay.Customer.Business.Gateway.Common.Models;

public class RequestHeaders
{
    public string CountryCode { get; set; } = string.Empty;
    public string LanguageId { get; set; } = string.Empty;
    public string? AccessToken { get; set; } = string.Empty;
    public BasicAuthentication? BasicAuth { get; set; }
    public string? BearerToken { get; set; }
    
    public bool HasBasicAuth => BasicAuth is not null;
    public bool HasBearerToken => BearerToken is not null;
    public bool HasCountryCode => !string.IsNullOrWhiteSpace(CountryCode);
    public bool HasLanguageId => !string.IsNullOrWhiteSpace(LanguageId);
    public bool HasAccessToken => AccessToken is not null;
    public bool HasRequiredParams => HasBearerToken || HasBasicAuth && !string.IsNullOrWhiteSpace(CountryCode) && !string.IsNullOrWhiteSpace(LanguageId);
    
    public bool HasRequiredParamsForMerchantOnboarding => 
        HasBasicAuth && HasCountryCode && HasLanguageId;
    
    public bool HasRequiredParamsForMerchantTransactions => 
        HasBearerToken && HasCountryCode || HasLanguageId;
}

public class BasicAuthentication
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RequestBody<T>
{
    public RequestPayloadType Type { get; set; } = RequestPayloadType.JSON;
    public T? Payload { get; set; }
}