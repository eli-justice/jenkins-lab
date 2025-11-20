namespace Egapay.Customer.Business.Gateway.Common.Utils.Enums;

public record CustomerStatus(string value)
{
    public static readonly CustomerStatus Unknown = new("UNKNOWN");
    public static readonly CustomerStatus Pending = new("PENDING");
    public static readonly CustomerStatus Active = new("ACTIVE");

    public override string ToString() => value;
}

public record BackOfficeAuthStatus(string value)
{
    public static readonly BackOfficeAuthStatus Editing = new("EDITING");
    public static readonly BackOfficeAuthStatus Reviewing = new("REVIEWING");
    public static readonly BackOfficeAuthStatus Passed = new("COMPLIANCE_PASSED");
    
    public override string ToString() => value.ToUpper();
};

public record InternalOrExternal(string value)
{
    public static readonly PersonalBusinessGroup Internal  = new ("INTERNAL");
    public static readonly PersonalBusinessGroup External  = new ("EXTERNAL");
    
    public override string ToString() => value.ToUpper();
};

public record VerifyStatus(string value)
{
    public static readonly PersonalBusinessGroup Verified  = new ("VERIFIED");
    public static readonly PersonalBusinessGroup UnVerified  = new ("UNVERIFIED");
    
    public override string ToString() => value.ToUpper();
};

public record CustomerPortalStatus(string value)
{
    public static readonly CustomerPortalStatus Pending  = new ("PENDING");
    public static readonly CustomerPortalStatus Reviewing  = new ("REVIEWING");
    public static readonly CustomerPortalStatus Editing  = new ("EDITING");
    
    public override string ToString() => value.ToUpper();
};

public record PasswordResetStatus(string value)
{
    public static readonly PasswordResetStatus Unknown  = new ("UNKNOWN");
    public static readonly PasswordResetStatus Temporal  = new ("TEMPORAL");
    public static readonly PasswordResetStatus Permanent  = new ("PERMANENT");
    
    public override string ToString() => value.ToUpper();
};