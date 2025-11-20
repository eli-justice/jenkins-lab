using OmniDataAccess.Core.Configs;

namespace Egapay.Customer.Business.Gateway.Common.Configs;

public class RedisCacheConfig:CacheManagerOptions
{
}

public class RedisServer
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int DatabaseIndex { get; set; }
}