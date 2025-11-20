using Egapay.Customer.Business.Gateway.API.Middlewares;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Controllers;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Services;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Egapay.Customer.Business.Gateway.API.Controllers.MerchantOnboarding;

[MerchantOnboardingAuthentication]
[ApiController]
[Route("api/v1/onboarding")]
public class CountryController:EganowControllerBase
{
    private readonly ILogger<CountryController> _logger;
    private readonly CommonService _commonService;
    public CountryController(ILogger<CountryController> logger,IOptions<ApplicationConfig> options, 
        IStringLocalizer<SuccessMessages> successLocalizer, IStringLocalizer<ErrorMessages> errorLocalizer,CommonService commonService)
    {
        _logger = logger;
        _config = options.Value;
        _successLocalizer = successLocalizer;
        _errorLocalizer = errorLocalizer;
        _commonService = commonService;
    }

    [HttpGet("countries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCountries([FromQuery]int filter)
    {
        try
        {
            var headers = GetRequestHeaders();
            //validate filter 
            if (filter == (int)CountryFilter.Unspecified)
            {
                return StatusCode(400,new ApiResponse<string>(400,_errorLocalizer["UnknownCountryFilter"]));
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var result = await _commonService.GetCountriesAsync((CountryFilter)filter,headers,cts.Token);

            var (response, errorResult) = result.ToTuple();
            if (errorResult != null || result.HasError)
            {
                return StatusCode((int)errorResult.StatusCode,new ApiResponse<string>((int)errorResult.StatusCode,errorResult.Message));
            }
            return StatusCode((int)response.Code,response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetCountries)} exception: {ex.Message}");
            return StatusCode(500,new ApiResponse<string>(500,_errorLocalizer["GetCountriesFailed"]));
        }
       
       
    }
    
    [HttpGet("regions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRegions()
    {
        try
        {
            var headers = GetRequestHeaders();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var result = await _commonService.GetRegionsAsync(headers,cts.Token);
            
            var (response, errorResult) = result.ToTuple();
            if (IsFailedResponse(errorResult))
            {
                return SetFailedResponse(errorResult);
            }

            return StatusCode((int)response.Code,response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetCountries)} exception: {ex.Message}");
            return StatusCode(500,new ApiResponse<string>(500,_errorLocalizer["GetRegionsFailed"]));
        }
       
       
    }
}