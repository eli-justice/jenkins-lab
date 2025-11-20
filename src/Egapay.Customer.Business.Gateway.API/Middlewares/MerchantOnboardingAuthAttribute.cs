using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.API.Middlewares;

public class MerchantOnboardingAuthentication : TypeFilterAttribute
{
    public MerchantOnboardingAuthentication() 
        : base(typeof(MerchantOnboardingAuthenticationFilter))
    {
    }
}
public class MerchantOnboardingAuthenticationFilter: EganowControllerBase,IAsyncActionFilter
{
    private readonly ILogger<MerchantOnboardingAuthenticationFilter> _logger;
    
    public MerchantOnboardingAuthenticationFilter(ILogger<MerchantOnboardingAuthenticationFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var headers = GetRequestHeaders(context.HttpContext.Request.Headers);

            if (headers != null && headers.HasRequiredParamsForMerchantOnboarding || headers.HasRequiredParamsForMerchantTransactions)
            {
                await next();
                return;
            }

            var response = new ApiResponse<string>(
                403, 
                "Missing or mismatch request headers"
            );

            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status403Forbidden,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(response)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(OnActionExecutionAsync)} exception: {ex.Message})");
            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(new ApiResponse<string>(500,"something went wrong"))
            };
        }
    }
}