using System.Net.Mime;
using Egapay.Customer.Business.Gateway.API.Middlewares;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Controllers;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
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
[Route("api/v1/onboarding/otp")]
public class OtpController: EganowControllerBase
{
    private readonly ILogger<OtpController> _logger;
    private readonly OtpService _otpService;
    public OtpController(ILogger<OtpController> logger,IOptions<ApplicationConfig> options,
        IStringLocalizer<SuccessMessages> successMessages, IStringLocalizer<ErrorMessages> errorMessages,
        IStringLocalizer<ValidationMessages> validationLocalizer,OtpService otpService)
    {
        _logger = logger;
        _config = options.Value;
        _successLocalizer = successMessages;
        _errorLocalizer = errorMessages;
        _validationLocalizer = validationLocalizer;
        _otpService = otpService;
    }
    
    [HttpPost]
    [Route("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Generate(GenerateOtpRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new GenerateOtpRequestValidator(_validationLocalizer))
            {
                if (request is null || string.IsNullOrEmpty(request.Email))
                {
                    request = new GenerateOtpRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(Generate)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _otpService.GenerateOtpAsync(request, GetRequestHeaders(), cts.Token);
            var (response, error) = respResult.ToTuple();
            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Generate)} exception: {ex.Message}");
            return StatusCode(500,new ApiResponse<string>(500,_errorLocalizer["DefaultError"]));
        }
    }
    
    [HttpPost]
    [Route("verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Verify(VerifyOtpRequest? request)
    {
        try
        {
            //validate fields
            using (var validator = new VerifyOtpRequestValidator(_validationLocalizer))
            {
                if (request is null || string.IsNullOrEmpty(request.Email))
                {
                    request = new VerifyOtpRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(Verify)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _otpService.VerifyOtpAsync(request, GetRequestHeaders(), cts.Token);
            var (response, error) = respResult.ToTuple();
            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }

            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Verify)} exception: {ex.Message}");
            return StatusCode(500,new ApiResponse<string>(500,_errorLocalizer["DefaultError"]));
        }
    }
}