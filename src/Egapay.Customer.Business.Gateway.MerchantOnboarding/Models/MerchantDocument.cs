using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;

public class AddBusinessDocumentRequest
{
    [FromForm(Name = "name")]
    public string Name { get; set; } = string.Empty;
    [FromForm(Name = "document")]
    public IFormFile? Document { get; set; }
}

public class UpdateBusinessDocumentRequest
{
    [FromForm(Name = "name")]
    public string Name { get; set; } = string.Empty;
    [FromForm(Name = "documentId")]
    public string DocumentId { get; set; }
    [FromForm(Name = "document")]
    public IFormFile? Document { get; set; }
}


public class BusinessDocument
{
    public string DocumentId { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
}

public class MerchantBusinessDocument
{
    [JsonProperty("documentId")]
    public int DocumentId { get; set; }

    [JsonProperty("customerGuid")]
    public string CustomerGuid { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("fileName")]
    public string FileName { get; set; }

    [JsonProperty("extension")]
    public string Extension { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("makerId")]
    public string MakerId { get; set; }
}