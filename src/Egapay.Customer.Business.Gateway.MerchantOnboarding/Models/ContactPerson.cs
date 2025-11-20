namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;

public class AddBusinessContactPersonRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string Position { get; set; }
}

public class UpdateBusinessContactPersonRequest
{
    public string ContactId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string Position { get; set; }
}


public class BusinessContactPerson
{
    public string ContactId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
}