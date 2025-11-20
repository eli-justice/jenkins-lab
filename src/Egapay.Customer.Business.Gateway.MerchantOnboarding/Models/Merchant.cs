using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;


public class CreateMerchantRequest
{
    [JsonProperty("personalInformation")]
    public PersonalInformation PersonalInformation { get; set; } = new PersonalInformation();
    [JsonProperty("businessInformation")]
    public BusinessInformation BusinessInformation { get; set; } = new BusinessInformation();
    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;
    [JsonProperty("otp")]
    public string Otp { get; set; } = string.Empty;
}

public class PersonalInformation
{
    [JsonProperty("firstName")]
    public string FirstName { get; set; }
    [JsonProperty("lastName")]
    public string LastName { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
}

public class BusinessInformation
{
    [JsonProperty("businessName")]
    public string BusinessName { get; set; }
    [JsonProperty("tradingName")]
    public string TradingName { get; set; }
    [JsonProperty("mobileNumber")]
    public string MobileNumber { get; set; }
}

public class UpdateBusinessContactRequest
{
    [JsonProperty("postalZipCode")]
    public string PostalZipCode { get; set; }

    [JsonProperty("streetAddress")]
    public string StreetAddress { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("provinceState")]
    public string ProvinceState { get; set; }

    [JsonProperty("digitalAddress")]
    public string DigitalAddress { get; set; }

    [JsonProperty("firstOccupancyDate")]
    public string? FirstOccupancyDate { get; set; } // first occupancy date (required, YYYY-MM-DD)

    [JsonProperty("officeOwnership")]
    public string OfficeOwnership { get; set; } // office ownership (required)

    [JsonProperty("postalAddress")]
    public string PostalAddress { get; set; } // postal address (required)

    [JsonProperty("officeMobileNumber")]
    public string OfficeMobileNumber { get; set; } // office mobile number (required)
}

public class UpdateMerchantBusinessInfoRequest
{
    [JsonProperty("companyName")]
    public string CompanyName { get; set; }

    [JsonProperty("tradingName")]
    public string TradingName { get; set; }
    
    [JsonProperty("dateOfIncorporation")]
    public string? DateOfIncorporation { get; set; }

    [JsonProperty("companyRegistrationNumber")]
    public string CompanyRegistrationNo { get; set; } 
    
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("regulatorId")]
    public string RegulatorId { get; set; }
    
    [JsonProperty("companyRegistrationType")]
    public string CompanyRegistrationType { get; set; }

    [JsonProperty("licenseInfo")] 
    public LicenseInfo LicenseInfo { get; set; } = new LicenseInfo();
    
    [JsonProperty("taxIdentificationNumber")]
    public string TaxIdentificationNumber { get; set; } 

    [JsonProperty("vatNumber")]
    public string? VATNumber { get; set; } 

    [JsonProperty("industryId")]
    public string IndustryId { get; set; } 
    
    [JsonProperty("industryName")]
    public string IndustryName { get; set; } 

    [JsonProperty("profilePicture")]
    public string ProfilePicture { get; set; }

    [JsonProperty("allowForEdit")]
    public bool AllowForEdit { get; set; }
}

public class LicenseInfo
{
    [JsonProperty("licenseNumber")] 
    public string LicenseNumber { get; set; } 

    [JsonProperty("licenseIssuedDate")]
    public string? IssuedDate { get; set; }

    [JsonProperty("licenseExpiryDate")]
    public string? ExpiryDate { get; set; }
}

public class ApproveUSSDMerchantCustomerRequest
{
    public string PayPartnerService {get; set;}
    public string AccountNumber {get; set;}
    public string Status {get; set;}
}

public class ApproveUSSDMerchantCustomerPinResetRequest
{
    public string PayPartnerService {get; set;}
    public string MobileNumber {get; set;}
    public string Status {get; set;}
}