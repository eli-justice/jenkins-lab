using System.Globalization;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Controllers;
using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.API.Middlewares;

public class LocalizationMiddleware:EganowControllerBase
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LocalizationMiddleware> _logger;

    public LocalizationMiddleware(RequestDelegate next, ILogger<LocalizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var headers = GetRequestHeaders(context.Request.Headers);
            var cultureQuery = headers.LanguageId;
            if (!string.IsNullOrEmpty(cultureQuery))
            {
                var cultureInfo = new CultureInfo(cultureQuery);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(InvokeAsync)} exception: {ex.Message})");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse<string>(500,"something went wrong")));
        }
    }
}