using Microsoft.Extensions.Diagnostics.HealthChecks;
using OmniDataAccess.NoSqlDatabases.Interfaces;

namespace Egapay.Customer.Business.Gateway.API.HealthChecks;

public class RedisCacheServiceHealthCheck:IHealthCheck
{
    private readonly ICacheManager _redisManager;
    public RedisCacheServiceHealthCheck(ICacheManager redisManager)
    {
        _redisManager = redisManager;
    }
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            
            var result =  _redisManager.Ping();
            
            var metadata = new Dictionary<string, object>()
            {
                {"name","onboarding-redis"},
            };

            if (!result)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("redis cache unavailable", data: metadata));
            }
            return Task.FromResult(HealthCheckResult.Healthy("redis cache ready", data: metadata));

            
        }
        catch (Exception ex)
        {
            var failure = HealthCheckResult.Unhealthy("redis cache check failure", exception: ex);
            return Task.FromResult(failure);
        }
    }
}