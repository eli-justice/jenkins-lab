namespace Egapay.Customer.Business.Gateway.Common.Configs;

public class ApplicationConfig
{
    public string Environment { get; set; } = string.Empty;
    public string ApiVersion {get;set;} = string.Empty;
    public string HmacSecretKey {get;set;} = string.Empty;
    public string JwtSecretKey {get;set;} = string.Empty;
    public string MerchantId {get;set;} = string.Empty;
    public string MerchantSecret {get;set;} = string.Empty;
    public int ContextTimeout { get; set; } = 30;
    public CorsPolicy CorsPolicy {get;set;}
}

public class CorsPolicy
{
    public List<string> AllowedOrigins { get; set; } = new List<string>();
}