namespace Egapay.Customer.Business.Gateway.Common.Models;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public Dictionary<string,object> Errors { get; set; }
}