using Egapay.Customer.Business.Gateway.Common.Models;

namespace Egapay.Customer.Business.Gateway.API.Interfaces;

public interface IAuthService:ILocalization
{
    Task<MerchantClaims?> GetAuthUserClaimsAsync(string token);
}