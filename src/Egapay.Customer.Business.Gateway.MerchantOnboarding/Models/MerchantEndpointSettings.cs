namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;

public class MerchantCallbackRequest
{
    public string? Service { get; set; }
    public string? PayoutCallbackUrl { get; set; }
    public string? CollectionCallbackUrl { get; set; }
}