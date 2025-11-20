using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Services;
using Egapay.Customer.Business.Gateway.Common.Utils;
using Egapay.Customer.Business.Gateway.Common.Utils.Constants;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using Egapay.Customer.Business.Gateway.Common.Utils.Routes;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Services;

public class OtpService
{
    private readonly ILogger<OtpService> _logger;
    private readonly ApiClientService _client;
    private readonly InternalServicesConfig _internalServicesConfig;

    public OtpService(ILogger<OtpService> logger, ApiClientService client, IOptions<InternalServicesConfig> internalServicesConfig)
    {
        _logger = logger;
        _client = client;
        _internalServicesConfig = internalServicesConfig.Value;
    }

    public async Task<OperationResult<ApiClientResponse?>> GenerateOtpAsync(GenerateOtpRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.GenerateOtpEndpoint);
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
            _logger.LogError($"{nameof(GenerateOtpAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GenerateOtpAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> VerifyOtpAsync(VerifyOtpRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.VerifyOtpEndpoint);
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
            _logger.LogError($"{nameof(VerifyOtpAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(VerifyOtpAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }

}