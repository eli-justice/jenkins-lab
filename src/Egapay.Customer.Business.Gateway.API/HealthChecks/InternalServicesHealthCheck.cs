using System.Net;
using System.Text.Json;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Services;
using Egapay.Customer.Business.Gateway.Common.Utils.Routes;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Environment = System.Environment;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Egapay.Customer.Business.Gateway.API.HealthChecks;

public class InternalServicesHealthCheck:IHealthCheck
{
    private readonly ApiClientService _clientService;
    private readonly InternalServicesConfig _internalServicesConfig;
    private readonly ILogger<InternalServicesHealthCheck> _logger;
    public InternalServicesHealthCheck(ILogger<InternalServicesHealthCheck> logger,ApiClientService clientService,IOptions<InternalServicesConfig> internalServicesConfig)
    {
        _clientService = clientService;
        _internalServicesConfig = internalServicesConfig.Value;
        _logger = logger;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var services = _internalServicesConfig.Services;
            var tasks = new List<Task<ApiClientResponse?>>();
            for (int i = 0;i< services.Count;i++)
            {
                string url = $"{services[i].Environment.BaseUrl.TrimEnd('/')}/{MerchantOnboardingRoutes.HealthChecksEndpoint}";
                
                tasks.Add(_clientService.MakeRequestAsync<JsonContent>(url, HttpMethod.Get));
            }

            var responses = await Task.WhenAll(tasks);

            var metadata = new Dictionary<string, object>()
            {
            };
            
            for (int i = 0; i < responses.Length; i++)
            {
                if (responses[i].Code != HttpStatusCode.OK)
                {
                    _logger.LogWarning($"failed to get successful response =>  service: {services[i].Name} response {JsonConvert.SerializeObject(responses[i])}");
                    metadata.Add(services[i].Name, "Internal services health check failed");
                }
                else
                {
                    Dictionary<string,object> body  = JsonSerializer.Deserialize<Dictionary<string,object>>(responses[i]?.Body,new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    
                    var data = body["data"];
                    
                    metadata.Add(services[i].Name, data);
                }
            }
            
            return await Task.FromResult(HealthCheckResult.Healthy("internal services ready", data: metadata));
        }
        catch (Exception ex)
        {
            var failure = HealthCheckResult.Unhealthy("internal services check failure", exception: ex);
            return await Task.FromResult(failure);
        }
    }
}