using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Egapay.Customer.Business.Gateway.API.HealthChecks;

public class WebApiServiceHealthCheck:IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var metadata = new Dictionary<string, object>()
            {
                // { "dotnetVersion", Environment.Version.ToString() },
                // { "processors", Environment.ProcessorCount },
                // { "osVersion", Environment.OSVersion.VersionString },
                // { "os", Environment.OSVersion.Platform.ToString() }
                {"name","merchant-business-gateway"}
            };

            return Task.FromResult(HealthCheckResult.Healthy("api service ready", data: metadata));
        }
        catch (Exception ex)
        {
            var failure = HealthCheckResult.Unhealthy("api service check failure", exception: ex);
            return Task.FromResult(failure);
        }
    }
}