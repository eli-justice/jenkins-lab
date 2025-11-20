namespace Egapay.Customer.Business.Gateway.Common.Configs;

public class InternalServicesConfig
{
    public List<AppService> Services { get; set; } =  new List<AppService>();
}

public class AppService
{
    public string Name { get; set; } = string.Empty;
    public Environment Environment {get;set;} = new Environment();
}

public class Environment
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public int ContextTimeout { get; set; } = 30;
    public int CurrentVersion { get; set; } = 1;
}