using System.Text.Json;
using Egapay.Customer.Business.Gateway.API.Features.Shared.Dtos;
using Egapay.Customer.Business.Gateway.API.HealthChecks;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Egapay.Customer.Business.Gateway.API.Extensions;

public static class HealthCheckExtensions
{
        public static void AddServiceHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<WebApiServiceHealthCheck>("server", tags: ["server","api"])
            .AddCheck<InternalServicesHealthCheck>("internalServices", tags: ["api","internal services"])
            .AddCheck<RedisCacheServiceHealthCheck>("redis", tags: ["redis","cache","datasource"]);
    }

    public static void MapServiceHealthChecks(this WebApplication app)
    {
        Func<HttpContext, HealthReport, Task> writeAction = (context, healthReport) =>
        {
            context.Response.ContentType = "application/json";
            var dto = new Dictionary<string, object>();
            
            //gateway report
            var report  = healthReport.Entries.Where(x=>x.Key == "server").Select(x=>x.Value).FirstOrDefault();
            dto.Add("server",new GatewayHealthCheckDto()
            {
                Name = report.Data["name"].ToString() ?? "",
                Description = report.Description,
                Duration = $"{report.Duration.TotalMilliseconds} ms",
                Status = report.Status.ToString(),
            });
            
            //redis report
            report  = healthReport.Entries.Where(x=>x.Key == "redis").Select(x=>x.Value).FirstOrDefault();
            dto.Add("redis",new GatewayHealthCheckDto()
            {
                Name = report.Data["name"].ToString() ?? "",
                Description = report.Description,
                Duration = $"{report.Duration.TotalMilliseconds} ms",
                Status = report.Status.ToString(),
            });
            
            //internal services report
            report  = healthReport.Entries.Where(x=>x.Key == "internalServices").Select(x=>x.Value).FirstOrDefault();
            dto.Add("internalServices",report.Data);
            

            var jsonDocument = JsonSerializer.Serialize(new ApiResponse<dynamic>(200,"health check report(s) retrieved successfully",dto),new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
    
            return context.Response.WriteAsync(jsonDocument);
        };
        
        app.MapHealthChecks("/health-checks",new HealthCheckOptions
        {
            ResponseWriter = writeAction
        });
    }
}