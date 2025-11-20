using System.Net.Mime;
using Egapay.Customer.Business.Gateway.API.Interfaces;
using Egapay.Customer.Business.Gateway.API.Middlewares;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Controllers;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.MerchantTransactions.Models;
using Egapay.Customer.Business.Gateway.MerchantTransactions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Egapay.Customer.Business.Gateway.API.Controllers.MerchantTransactions;

[MerchantTransactionAuthentication]
[ApiController]
[Route("api/v1/merchant-transactions/payments")]
public class PaymentController:EganowControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IAuthService _authService;
    private readonly PaymentService _paymentService;
    
    public PaymentController(ILogger<PaymentController> logger,IOptions<ApplicationConfig> options,
        IStringLocalizer<SuccessMessages> successMessages, IStringLocalizer<ErrorMessages> errorMessages,
        IStringLocalizer<ValidationMessages> validationLocalizer,IAuthService authService,PaymentService paymentService)
    {
        _logger = logger;
        _config = options.Value;
        _successLocalizer = successMessages;
        _errorLocalizer = errorMessages;
        _validationLocalizer = validationLocalizer;
        _authService = authService;
        _paymentService = paymentService;
    }
    
    [HttpGet("paypartners")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetPaypartners([FromQuery]string service)
    {
        try
        {
            //extract jwt from Bearer token
            var authHeaders = GetRequestHeaders();
            var merchantClaims = await _authService.GetAuthUserClaimsAsync(authHeaders.BearerToken);

            if (merchantClaims == null)
            {
                return StatusCode(401, new ApiResponse<string>(401, _errorLocalizer["UnAuthorizedUser"]));
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _paymentService.GetPayPartnersAsync(service,authHeaders,null,cts.Token);
            
            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetPaypartners)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
    }
    
    [HttpPost("name-enquiry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetNameEnquiry(NameEnquiryRequest? request)
    {
        try
        {
            //extract jwt from Bearer token
            var authHeaders = GetRequestHeaders();
            var merchantClaims = await _authService.GetAuthUserClaimsAsync(authHeaders.BearerToken);

            if (merchantClaims == null)
            {
                return StatusCode(401, new ApiResponse<string>(401, _errorLocalizer["UnAuthorizedUser"]));
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _paymentService.GetNameEnquiryAsync(request,authHeaders,cts.Token);

            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetNameEnquiry)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
    }
    
    
    [HttpPost("collection")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> InitialCollectionPayment(InitiateCollectionPaymentRequest? request)
    {
        try
        {
            //extract jwt from Bearer token
            var authHeaders = GetRequestHeaders();
            var merchantClaims = await _authService.GetAuthUserClaimsAsync(authHeaders.BearerToken);

            if (merchantClaims == null)
            {
                return StatusCode(401, new ApiResponse<string>(401, _errorLocalizer["UnAuthorizedUser"]));
            }
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _paymentService.InitiateCollectionAsync(request,authHeaders,cts.Token);
            
            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(InitialCollectionPayment)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
        
    }
    
    [HttpPost("payout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> InitialPayoutPayment(InitiatePayoutPaymentRequest? request)
    {
        try
        {
            //extract jwt from Bearer token
            var authHeaders = GetRequestHeaders();
            var merchantClaims = await _authService.GetAuthUserClaimsAsync(authHeaders.BearerToken);

            if (merchantClaims == null)
            {
                return StatusCode(401, new ApiResponse<string>(401, _errorLocalizer["UnAuthorizedUser"]));
            }
        
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _paymentService.InitiatePayoutAsync(request,authHeaders,cts.Token);

            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(InitialPayoutPayment)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
        
    }
    
    [HttpPost("charges")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetPaymentCharges(PaymentChargesRequest? request)
    {
        try
        {
            //extract jwt from Bearer token
            var authHeaders = GetRequestHeaders();
            var merchantClaims = await _authService.GetAuthUserClaimsAsync(authHeaders.BearerToken);

            if (merchantClaims == null)
            {
                return StatusCode(401, new ApiResponse<string>(401, _errorLocalizer["UnAuthorizedUser"]));
            }
        
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _paymentService.GetPaymentChargesAsync(request,authHeaders,cts.Token);

            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetPaymentCharges)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
        
    }
}