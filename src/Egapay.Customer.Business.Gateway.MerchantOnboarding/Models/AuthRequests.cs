using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;

public class LoginMerchantRequest
{
    [JsonProperty("email")]
    public string Email { get; set; } = String.Empty;
    
    [JsonProperty("password")]
    public string Password { get; set; } = String.Empty;

    public LoginMerchantRequest(){}

    public LoginMerchantRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
}

public class RequestPasswordResetRequest
{
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;
}

public class PasswordResetRequest:LoginMerchantRequest
{
}

public class CheckMerchantRequest
{
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("mobileNumber")]
    public string MobileNumber { get; set; }
}

public class GenerateOtpRequest
{
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("mobileNumber")]
    public string? MobileNumber { get; set; }
    
    [JsonProperty("purpose")]
    public string Purpose { get; set; }
}

public class VerifyOtpRequest
{
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("otp")]
    public string Otp { get; set; }
    
    [JsonProperty("purpose")]
    public string Purpose { get; set; }
}