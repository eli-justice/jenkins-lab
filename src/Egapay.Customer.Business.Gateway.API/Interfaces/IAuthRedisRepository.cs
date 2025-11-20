
using Egapay.Customer.Business.Gateway.Common.Models;

namespace Egapay.Customer.Business.Gateway.API.Interfaces;

public interface IAuthRedisRepository:ILocalization
{
    Task<OperationResult<bool>> SaveAuthTokenAsync(string key,string token);
    Task<OperationResult<string?>> GetAuthTokenAsync(string key);
}