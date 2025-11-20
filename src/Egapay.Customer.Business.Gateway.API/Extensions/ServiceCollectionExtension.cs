using System.Threading.RateLimiting;
using Egapay.Customer.Business.Gateway.API.Interfaces;
using Egapay.Customer.Business.Gateway.API.Services;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Controllers;
using Egapay.Customer.Business.Gateway.Common.Services;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Services;
using Egapay.Customer.Business.Gateway.MerchantTransactions.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.API.Extensions;

public static class ServiceCollectionExtension
{
    public static T GetService<T>(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetService<T>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ApiClientService>();
        services.AddTransient<IAuthService,AuthService>();
        services.AddScoped<TransactionService>();
        services.AddScoped<OnboardingAuthService>();
        services.AddScoped<OtpService>();
        services.AddScoped<PaymentService>();
        services.AddScoped<CommonService>();
        services.AddScoped<MerchantService>();
    }

    public static void AddConfigurations(this IServiceCollection services,WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApplicationConfig>(builder.Configuration.GetSection(nameof(ApplicationConfig)));
        builder.Services.Configure<InternalServicesConfig>(builder.Configuration.GetSection(nameof(InternalServicesConfig)));
        builder.Services.Configure<RedisCacheConfig>(builder.Configuration.GetSection(nameof(RedisCacheConfig)));

        var config = services.GetService<IOptions<ApplicationConfig>>().Value;
        Console.WriteLine($"ENVIRONMENT:  {config.Environment}");
        EganowControllerBase.MerchantId = config.MerchantId;
        EganowControllerBase.MerchantSecret = config.MerchantSecret;
        
        builder.Services.AddHttpClient("GatewayHttpClient", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(config.ContextTimeout);
            client.DefaultRequestHeaders.Add("Accept", "application/json"); // base default
        });
    }

    public static void AddCrossOrigins(this IServiceCollection services)
    {
        var config = services.GetService<IOptions<ApplicationConfig>>().Value;
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllCorsPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader() // Allows all headers including Authorization
                    .WithExposedHeaders("Authorization"); // Optional, if you need to expose it
                //.AllowCredentials(); // If you need to allow credentials             
            });
        });
    }

    public static void AddRateLimitingPolicy(this IServiceCollection services)
    {
        // Add services for rate limiting
        services.AddRateLimiter(options =>
        {
            // Add a sliding window rate limiter for all requests
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    // Partitioning by IP address or any other key you choose
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: partitionKey => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 50, // Allow 1000 requests
                        Window = TimeSpan.FromMinutes(1), // Set 1 minute time window
                        SegmentsPerWindow = 6, // Divide into 6 segments of 10 seconds each
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst, // Oldest queued requests are processed first
                        QueueLimit = 0 // Set queue limit to 0 to immediately return 429 when limit is hit
                    }
                )
            );

            // Customize the response when the limit is hit
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse<string>(429,"rate limit exceeded. please try again later.")), cancellationToken);
            };
        });
    }
}