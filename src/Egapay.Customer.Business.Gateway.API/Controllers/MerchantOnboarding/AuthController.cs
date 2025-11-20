using System.Net.Mime;
using Egapay.Customer.Business.Gateway.API.Middlewares;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Controllers;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Utils;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Services;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.API.Controllers.MerchantOnboarding;

[MerchantOnboardingAuthentication]
[ApiController]
[Route("api/v1/onboarding/auth")]
public class AuthController:EganowControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly OnboardingAuthService _authService;

    public AuthController(ILogger<AuthController> logger,IOptions<ApplicationConfig> options,
        IStringLocalizer<SuccessMessages> successMessages, IStringLocalizer<ErrorMessages> errorMessages,
        IStringLocalizer<ValidationMessages> validationLocalizer, OnboardingAuthService authService)
    {
        _logger = logger;
        _config = options.Value;
        _successLocalizer = successMessages;
        _errorLocalizer = errorMessages;
        _validationLocalizer = validationLocalizer;
        _authService = authService;
    }
    
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> LoginMerchant(LoginMerchantRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new LoginMerchantRequestValidator(_validationLocalizer))
            {
                if (request is null || string.IsNullOrEmpty(request.Email))
                {
                    request = new LoginMerchantRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(LoginMerchant)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new ApiResponse<Dictionary<string,object>>(400,_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult =
                await _authService.LoginMerchantAsync(request,GetRequestHeaders(),cts.Token);

            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(LoginMerchant)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["AuthenticateMerchantFailed"]));
        }
    }
    
    [HttpPost("request-password-reset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> RequestPasswordReset(RequestPasswordResetRequest? request)
    {
        try
        {
            string email = request?.Email;
            var errorList = new List<string>();
            if (string.IsNullOrWhiteSpace(email))
            {
                errorList.Add(_validationLocalizer["EmailNotEmpty"]);
            }

            if (!UtilHelpers.IsValidEmail(email))
            {
                errorList.Add(_validationLocalizer["InvalidEmail"]);
            }

            if (errorList.Any())
            {
                var dic = new Dictionary<string, List<string>>();
                dic.Add("email", errorList);
                return StatusCode(400,new BadRequestResponse<Dictionary<string, List<string>>>(_errorLocalizer["ValidationErrors"],dic));
            }
            
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult =
                await _authService.RequestPasswordResetAsync(request,GetRequestHeaders(),cts.Token);

            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);

        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(RequestPasswordReset)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
    }
    
    [HttpPost("password-reset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> PasswordReset(PasswordResetRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new PasswordResetRequestValidator(_validationLocalizer))
            {
                if (request is null || string.IsNullOrEmpty(request.Email))
                {
                    request = new PasswordResetRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(LoginMerchant)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
        
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult =
                await _authService.ResetMerchantPasswordAsync(request,GetRequestHeaders(),cts.Token);

            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }
            

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(RequestPasswordReset)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["ResetPasswordFailed"]));
        }
    }
}