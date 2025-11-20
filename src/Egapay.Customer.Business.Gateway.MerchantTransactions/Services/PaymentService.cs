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

public class PaymentService
{
    private readonly ILogger<PaymentService> _logger;
    private readonly ApiClientService _client;
    private readonly InternalServicesConfig _internalServicesConfig;

    public PaymentService(ILogger<PaymentService> logger, ApiClientService client, IOptions<InternalServicesConfig> internalServicesConfig)
    {
        _logger = logger;
        _client = client;
        _internalServicesConfig = internalServicesConfig.Value;
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetPayPartnersAsync(string paypartnerService,RequestHeaders headers,PaginationOption? pagination = null,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.GetPaypartnersEndpoint}?&service={paypartnerService}");
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
            _logger.LogError($"{nameof(GetPayPartnersAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetPayPartnersAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetNameEnquiryAsync(NameEnquiryRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.GetNameEnquiryEndpoint}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(GetNameEnquiryAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetNameEnquiryAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    public async Task<OperationResult<ApiClientResponse?>> InitiateCollectionAsync(InitiateCollectionPaymentRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.InitiateCollectionEndpoint}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(InitiateCollectionAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(InitiateCollectionAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    public async Task<OperationResult<ApiClientResponse?>> InitiatePayoutAsync(InitiatePayoutPaymentRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.InitiatePayoutEndpoint}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(InitiatePayoutAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(InitiatePayoutAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    public async Task<OperationResult<ApiClientResponse?>> GetPaymentChargesAsync(PaymentChargesRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.PaymentChargesEndpoint}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);

            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(GetPaymentChargesAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetPaymentChargesAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }

}