namespace Egapay.Customer.Business.Gateway.Common.Utils.Enums;

public enum CompanyRegistrationType
{
    UNKNOWN = 0,
    LIMITED_LIABILITY,
    PUBLIC_LIABILITY,
    SOLE_PROPRIETORSHIP,
    PARTNERSHIP,
    CORPORATION,
    C_CORPORATION,
    S_COPRORATION,
    JOINT_VENTURE,
    NONPROFIT,
}

public enum CustomerIdType
{
    UNKNOWN = 0,
    PASSPORT = 1,
    // DRIVERS_LICENSE = 2,
    ID = 3,
    // BANKID = 4,
}