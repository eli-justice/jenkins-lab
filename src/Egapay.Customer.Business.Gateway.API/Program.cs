using System.Text.Json;
using Egapay.Customer.Business.Gateway.API.Extensions;
using Egapay.Customer.Business.Gateway.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Egapay.Customer.Business.Gateway.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddConfigurations(builder);
        builder.Services.AddLocalization();
        var supportedCultures = new[] { "en", "fr" };

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
            options.SupportedCultures = supportedCultures.Select(c => new System.Globalization.CultureInfo(c)).ToList();
            options.SupportedUICultures = supportedCultures.Select(c => new System.Globalization.CultureInfo(c)).ToList();
        });
        
        //add rate limiting
        builder.Services.AddRateLimitingPolicy();
        
        // Add services to the container.
        builder.Services.AddCrossOrigins();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });


        builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        
        builder.Services.AddRepositories();
        builder.Services.AddServices();
        builder.Services.AddServiceHealthChecks();// health checks

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
                
        var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(locOptions.Value);

        app.UseHttpsRedirection();

        app.UseCors("AllowAllCorsPolicy");
        app.UseAuthorization();
        app.UseRateLimiter();
        app.MapControllers();
        app.UseMiddleware<LocalizationMiddleware>();
        app.MapServiceHealthChecks();
        
        app.MapGet("/", () => $"Welcome to Egapay Merchant Business Gateway API Service ({app.Configuration.GetSection("ApplicationConfig:Environment").Value})");

        app.Run();
    }
}