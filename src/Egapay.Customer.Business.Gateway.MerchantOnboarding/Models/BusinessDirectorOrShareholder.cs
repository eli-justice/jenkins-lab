using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;

public class AddBusinessDirectorOrShareholderRequest
{
    [FromForm(Name="firstName")]
    public string FirstName { get; set; }  = string.Empty;

    [FromForm(Name="lastName")]
    public string LastName { get; set; }  = string.Empty;

    [FromForm(Name="mobileNumber")]
    public string MobileNumber { get; set; }  = string.Empty;

    [FromForm(Name="email")]
    public string Email { get; set; }  = string.Empty;

    [FromForm(Name="position")]
    public string Position { get; set; }  = string.Empty;

    [FromForm(Name="number")]
    public string Number { get; set; }

    [FromForm(Name="expiryDate")]
    public string? ExpiryDate { get; set; } // Use string for date representation (e.g., "YYYY-MM-DD")

    [FromForm(Name="placeOfIssue")]
    public string PlaceOfIssue { get; set; }

    [FromForm(Name="type")]
    public string Type { get; set; }

    [FromForm(Name="frontImage")]
    public IFormFile FrontImage { get; set; }

    [FromForm(Name="backImage")]
    public IFormFile? BackImage { get; set; }

    [FromForm(Name="portraitImage")]
    public IFormFile PortraitImage { get; set; }
}

public class UpdateBusinessDirectorOrShareholderRequest:AddBusinessDirectorOrShareholderRequest
{
    [FromForm(Name = "directorId")]
    public string DirectorId { get; set; }
}

public class DirectorShareholderIdInformation
{
    [JsonProperty("number")]
    public string Number { get; set; }

    [JsonProperty("expiryDate")]
    public string? ExpiryDate { get; set; } // Use string for date representation (e.g., "YYYY-MM-DD")

    [JsonProperty("placeOfIssue")]
    public string PlaceOfIssue { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("frontImage")]
    public string FrontImage { get; set; }

    [JsonProperty("backImage")]
    public string BackImage { get; set; }

    [JsonProperty("portraitImage")]
    public string PortraitImage { get; set; }
}

public class DirectorShareholderIdentification
{
    [JsonProperty("number")]
    public string Number { get; set; }

    [JsonProperty("expiryDate")]
    public string ExpiryDate { get; set; } // Use string for date representation (e.g., "YYYY-MM-DD")

    [JsonProperty("placeOfIssue")]
    public string PlaceOfIssue { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("frontImage")]
    public string FrontImage { get; set; }

    [JsonProperty("backImage")]
    public string BackImage { get; set; }

    [JsonProperty("portraitImage")]
    public string PortraitImage { get; set; }
}


public class DirectorOrShareholder
{
    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }
    
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("mobileNumber")]
    public string MobileNumber { get; set; }
    
    public DirectorShareholderIdentification Identification { get; set; }
    
    [JsonProperty("directorPosition")]
    public string Position { get; set; }

    [JsonProperty("directorOrShareholderOrOtherType")]
    public string DirectorOrShareholderOrOtherType { get; set; }
    
    [JsonProperty("directorId")]
    public string DirectorId { get; set; }
    
    [JsonProperty("passedAmlCheck")]
    public bool PassedAmlCheck { get; set; }
    
}