namespace Egapay.Customer.Business.Gateway.Common.Utils.Enums;

public record MobileOrWebValue(string value)
{
    public static readonly MobileOrWebValue Mobile  = new ("MOBILE");
    public static readonly MobileOrWebValue Web  = new ("WEB");
    
    public override string ToString() => value.ToUpper();
}