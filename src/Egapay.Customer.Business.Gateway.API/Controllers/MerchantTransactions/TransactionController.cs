using System.Net.Mime;
using Egapay.Customer.Business.Gateway.API.Interfaces;
using Egapay.Customer.Business.Gateway.API.Middlewares;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Controllers;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Services;
using Egapay.Customer.Business.Gateway.MerchantTransactions.Models;
using Egapay.Customer.Business.Gateway.MerchantTransactions.Services;
using Egapay.Customer.Business.Gateway.MerchantTransactions.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.API.Controllers.MerchantTransactions;

[MerchantTransactionAuthentication]
[ApiController]
[Route("api/v1/merchant-transactions")]
public class TransactionController:EganowControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly IAuthService _authService;
    private readonly TransactionService _transactionService;
    private readonly MerchantService _merchantService;
    
    public TransactionController(ILogger<TransactionController> logger,IOptions<ApplicationConfig> options,
        IStringLocalizer<SuccessMessages> successMessages, IStringLocalizer<ErrorMessages> errorMessages,
        IStringLocalizer<ValidationMessages> validationLocalizer,IAuthService authService,TransactionService transactionService, MerchantService merchantService)
    {
        _logger = logger;
        _config = options.Value;
        _successLocalizer = successMessages;
        _errorLocalizer = errorMessages;
        _validationLocalizer = validationLocalizer;
        _authService = authService;
        _transactionService = transactionService;
        _merchantService = merchantService;
    }
    
    [HttpGet("dashboard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> MerchantDashboard([FromQuery]string service,[FromQuery]string startDate, [FromQuery]string endDate)
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
            var result =
                await _transactionService.GetMerchantDashboardAsync(new TransactionReportRequest(startDate, endDate, service),
                    authHeaders,cts.Token);
            if (IsFailedResponse(result.GetError()))
            {
                return SetFailedResponse(result.GetError());
            }

            var response = result.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAccountTransactions)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["GetTransactionsFailed"]));
        }
    }
    
    [HttpPost("account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAccountTransactions([FromBody]AccountTransactionRequest? request,[FromQuery]int limit,[FromQuery]int page = 1)
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
            
            //validate fields
            using (var validator = new AccountTransactionRequestValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new AccountTransactionRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(GetAccountTransactions)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new ApiResponse<Dictionary<string,object>>(400,_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            PaginationOption? pagination = null;

            if (limit > 0 && page > 0)
            {
                pagination = new PaginationOption(page, limit);
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult =
                await _transactionService.GetAllAccountTransactionsAsync(request,authHeaders,pagination,cts.Token);
            
            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAccountTransactions)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["GetTransactionsFailed"]));
        }
    }
    
    [HttpGet("services")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetMerchantServices()
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
            var respResult = await _merchantService.GetAllMerchantServicesAsync(GetRequestHeaders(),cts.Token);

            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetMerchantServices)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
    }
    
    [HttpPost("fund-wallet/request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> FundMerchantWallet(MerchantFundWalletRequest? request)
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
            
            //validate fields
            using (var validator = new MerchantFundWalletRequestValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new MerchantFundWalletRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(FundMerchantWallet)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult =
                await _transactionService.FundMerchantWalletAsync(request,authHeaders,cts.Token);
            
            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(FundMerchantWallet)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
    }
    
    [HttpPost("fund-wallet")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetFundMerchantWallet([FromBody]GetMerchantFundWalletRequest? request,[FromQuery]int limit,[FromQuery]int page = 1)
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
            
            //validate fields
            using (var validator = new GetMerchantFundWalletRequestValidator(_validationLocalizer))
            {
                if (request is null)
                {
                    request = new GetMerchantFundWalletRequest();
                }
                
                var result = await validator.ValidateObjectAsync(request);
                if (!result.IsValid)
                {
                    _logger.LogWarning($"{nameof(FundMerchantWallet)} failed validation: {JsonConvert.SerializeObject(result.Errors)}");
                    return StatusCode(400, new BadRequestResponse<Dictionary<string,object>>(_errorLocalizer["ValidationErrors"],result.Errors));
                }
            }
            
            PaginationOption? pagination = null;

            if (limit > 0 && page > 0)
            {
                pagination = new PaginationOption(page, limit);
            }


            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult =
                await _transactionService.GetFundMerchantWalletsAsync(request,authHeaders,pagination,cts.Token);
            
            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(FundMerchantWallet)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
    }
    
    [HttpGet("balances")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetMerchantAccountBalances([FromQuery]string paypartnerService)
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
            var respResult =
                await _transactionService.GetMerchantBalancesMessageAsync(paypartnerService,authHeaders,cts.Token);
            
            if (IsFailedResponse(respResult.GetError()))
            {
                return SetFailedResponse(respResult.GetError());
            }

            var response = respResult.GetValue();
            return StatusCode((int)response.Code, response.Body);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetMerchantAccountBalances)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult(_errorLocalizer["DefaultError"]));
        }
    }
    
    [HttpGet]
    [Route("ussd-customers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetUSSDMerchantCustomers([FromQuery]string paypartnerService,[FromQuery]string status)
    {
        try
        {
            var authHeaders = GetRequestHeaders();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetUSSDMerchantCustomersAsync(paypartnerService,status,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }


            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetUSSDMerchantCustomers)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, "failed to get ussd merchant customers. try again later"));
        }
    }
    
    [HttpPost("ussd-customers/review")]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> ApproveUSSDMerchantCustomer(ApproveUSSDMerchantCustomerRequest? request)
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
            var respResult = await _merchantService.ApproveUSSDMerchantCustomerAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }


            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetUSSDMerchantCustomers)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult("failed to update customer status. try again later"));
        }
    }
    
    [HttpGet]
    [Route("ussd-customers/pin-reset-requests")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetUSSDMerchantCustomerPinResetRequests([FromQuery]string paypartnerService,[FromQuery]string status)
    {
        try
        {
            var authHeaders = GetRequestHeaders();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_config.ContextTimeout));
            var respResult = await _merchantService.GetUSSDMerchantCustomerPinResetRequestsAsync(paypartnerService,status,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }


            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetUSSDMerchantCustomerPinResetRequests)} exception: {ex.Message}");
            return StatusCode(500, new ApiResponse<string>(500, "failed to get ussd pin reset requests. try again later"));
        }
    }
    
    [HttpPost("ussd-customers/pin-reset-requests/review")]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> ApproveUSSDMerchantCustomerPinRequest(ApproveUSSDMerchantCustomerPinResetRequest? request)
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
            var respResult = await _merchantService.ApproveUSSDMerchantCustomerPinResetAsync(request,GetRequestHeaders(),cts.Token);
            
            var (response, error) = respResult.ToTuple();

            if (IsFailedResponse(error))
            {
                return SetFailedResponse(error);
            }


            return StatusCode((int)response.Code, response.Body); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetUSSDMerchantCustomers)} exception: {ex.Message}");
            return SetFailedResponse(new ErrorResult("failed to reset ussd pin. try again later"));
        }
    }
}