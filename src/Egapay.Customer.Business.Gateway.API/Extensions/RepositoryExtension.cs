using Egapay.Customer.Business.Gateway.API.Interfaces;
using Egapay.Customer.Business.Gateway.API.Repositories;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Microsoft.Extensions.Options;
using OmniDataAccess.NoSqlDatabases.Extensions;
using OmniDataAccess.NoSqlDatabases.Interfaces;

namespace Egapay.Customer.Business.Gateway.API.Extensions;

public static class RepositoryExtensions
{
    
    public static void AddRedisCache(this IServiceCollection services)
    {
        var redisOps = services.GetService<IOptions<RedisCacheConfig>>();
        services.AddRedisCacheManager(redisOps.Value);
        var manager = ServiceCollectionExtension.GetService<ICacheManager>(services); 
        manager.PingAsync();
    }


    public static void AddRepositories(this IServiceCollection services)
    {
        AddRedisCache(services);
        services.AddTransient<IAuthRedisRepository, AuthRedisRepository>();
    }
}