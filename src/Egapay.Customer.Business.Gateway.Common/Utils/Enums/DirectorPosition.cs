namespace Egapay.Customer.Business.Gateway.Common.Utils.Enums;
public enum DirectorPosition {
    UNKNOWN = 0,
    DIRECTOR = 1,
    SHAREHOLDER = 2,
    CEO = 3,
    MANAGEMENT = 4,
    ENGINEER = 5,
    MID = 6,
    CONTACT = 7,
    SIGNATORY = 9,
    GENERAL_MANAGER = 10,
    DIRECTOR_SHAREHOLDER = 11,
    SALES_MANAGER = 12,
}

// public record DirectorPosition(string value)
// {
//     public static DirectorPosition UNKNOWN = new("UNKNOWN");
//     public static DirectorPosition DIRECTOR = new("DIRECTOR");
//     public static DirectorPosition SHAREHOLDER = new("SHAREHOLDER");
//     public static DirectorPosition CEO = new("CEO");
//     public static DirectorPosition MANAGEMENT = new("MANAGEMENT");
//     public static DirectorPosition ENGINEER = new("ENGINEER");
//     public static DirectorPosition MID = new("MID");
//     public static DirectorPosition CONTACT = new("CONTACT");
//     public static DirectorPosition SIGNATORY = new("SIGNATORY");
//     public static DirectorPosition GENERAL_MANAGER = new("GENERAL_MANAGER");
//     public static DirectorPosition DIRECTOR_SHAREHOLDER = new("DIRECTOR_SHAREHOLDER");
//     public static DirectorPosition SALES_MANAGER = new("SALES_MANAGER");
//     public override string ToString() => value;
// }

public enum DirectorOrShareholderOrOtherType
{
    UNKNOWN = 0,
    CONTACT_PERSON = 1,
    DIRECTOR_SHAREHOLDER = 2,
}