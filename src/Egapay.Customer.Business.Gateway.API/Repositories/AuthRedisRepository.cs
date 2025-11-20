using Egapay.Customer.Business.Gateway.API.Interfaces;
using Egapay.Customer.Business.Gateway.Common.Extensions;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Utils;
using Microsoft.Extensions.Localization;
using OmniDataAccess.NoSqlDatabases.Interfaces;

namespace Egapay.Customer.Business.Gateway.API.Repositories;

public class AuthRedisRepository:RepositoryBase,IAuthRedisRepository
{
    private readonly ILogger<AuthRedisRepository> _logger;
    private string _redisNotConnected = "redis instance not connected.";
    private const int _index = 2;


    public AuthRedisRepository(ICacheManager redisManager, ILogger<AuthRedisRepository> logger,
        IStringLocalizer<SuccessMessages> successMessages, IStringLocalizer<ErrorMessages> errorMessages)
    {
        _redisManager = redisManager;
        _logger = logger;
        _successLocalizer = successMessages;
        _errorLocalizer = errorMessages;
    }

    public async Task<OperationResult<bool>> SaveAuthTokenAsync(string key,string token)
    {
        try
        {
            if (!_redisManager.Ping())
            {
                return new OperationResult<bool>(false, _redisNotConnected.ToErrorResult());
            }
            
            var result = await _redisManager.SaveAsStringAsync(key,token,database:_index,TimeSpan.FromHours(1));
            return new OperationResult<bool>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(SaveAuthTokenAsync)}: {ex.Message}");
            return new OperationResult<bool>(false, new ErrorResult(Translators.GetLocalizedMessage(_errorLocalizer,"DefaultError",_culture)));
        }
    }
    
    public async Task<OperationResult<string?>> GetAuthTokenAsync(string key)
    {
        try
        {
            if (!_redisManager.Ping())
            {
                return new OperationResult<string?>(_redisNotConnected.ToErrorResult());
            }
            
            var result = await _redisManager.GetStringAsync<string>(key, _index);
            return new OperationResult<string?>(result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAuthTokenAsync)}: {ex.Message}");
            return new OperationResult<string?>( new ErrorResult(Translators.GetLocalizedMessage(_errorLocalizer,"DefaultError",_culture)));
        }
    }
    
    public void SetCulture(string languageId)
    {
        _culture = languageId;
    }
}