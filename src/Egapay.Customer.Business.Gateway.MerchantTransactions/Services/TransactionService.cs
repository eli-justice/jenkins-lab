using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Services;
using Egapay.Customer.Business.Gateway.Common.Utils;
using Egapay.Customer.Business.Gateway.Common.Utils.Constants;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using Egapay.Customer.Business.Gateway.Common.Utils.Routes;
using Egapay.Customer.Business.Gateway.MerchantTransactions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Egapay.Customer.Business.Gateway.MerchantTransactions.Services;

public class TransactionService
{
    private readonly ILogger<TransactionService> _logger;
    private readonly ApiClientService _client;
    private readonly InternalServicesConfig _internalServicesConfig;

    public TransactionService(ILogger<TransactionService> logger, ApiClientService client, IOptions<InternalServicesConfig> internalServicesConfig)
    {
        _logger = logger;
        _client = client;
        _internalServicesConfig = internalServicesConfig.Value;
    }

    public async Task<OperationResult<ApiClientResponse?>> GetMerchantDashboardAsync(TransactionReportRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.DashboardEndpoint}?startDate={request.StartDate}&endDate={request.EndDate}&service={request.Service}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(GetMerchantDashboardAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetMerchantDashboardAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetAllAccountTransactionsAsync(AccountTransactionRequest request,RequestHeaders headers,PaginationOption? pagination = null,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string endpoint = MerchantTransactionRoutes.AccountTransactionEndpoint;

            if (pagination != null)
            {
                endpoint +=$"?page={pagination.Page}&limit={pagination.Limit}";
            }
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                endpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request),
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(GetAllAccountTransactionsAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllAccountTransactionsAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }

    public async Task<OperationResult<ApiClientResponse?>> FundMerchantWalletAsync(MerchantFundWalletRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantTransactionRoutes.FundMerchantWalletEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request),
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(GetAllAccountTransactionsAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllAccountTransactionsAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }

    public async Task<OperationResult<ApiClientResponse?>> GetFundMerchantWalletsAsync(GetMerchantFundWalletRequest request,RequestHeaders headers,PaginationOption? pagination = null,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string endpoint = MerchantTransactionRoutes.GetFundMerchantWalletEndpoint;

            if (pagination != null)
            {
                endpoint +=$"?page={pagination.Page}&limit={pagination.Limit}";
            }
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                endpoint);
            
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request),
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(GetFundMerchantWalletsAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetFundMerchantWalletsAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }

    public async Task<OperationResult<ApiClientResponse?>> GetMerchantBalancesMessageAsync(string payPartnerService,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.AccountBalancesEndpoint}?paypartnerService={payPartnerService}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
            };
            
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);

            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(GetMerchantBalancesMessageAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetMerchantBalancesMessageAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }

}