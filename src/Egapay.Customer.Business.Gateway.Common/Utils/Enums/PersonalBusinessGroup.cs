namespace Egapay.Customer.Business.Gateway.Common.Utils.Enums;

public record PersonalBusinessGroup(string value)
{
    public static readonly PersonalBusinessGroup Personal  = new ("PERSONAL");
    public static readonly PersonalBusinessGroup Business  = new ("BUSINESS");
    
    public override string ToString() => value.ToUpper();
};

public enum PersonalBusinessGroupValue
{
    UNKNOWN = 0,
    PERSONAL,
    BUSINESS,
}

public enum GroupValue
{
    UNKNOWN = 0,
    PERSONAL,
    BUSINESS,
}

public enum OfficeOwnership
{
    UNKNOWN = 0,
    OWNED = 1,
    RENT = 2,
    LEASED = 3,
}