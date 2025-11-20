using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;

public class UpdateMerchantProfileImageRequest
{
    [FromForm(Name = "name")]
    public string Name { get; set; } = string.Empty;
    [FromForm(Name = "image")]
    public IFormFile? Image { get; set; }
}