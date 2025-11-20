using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Services;
using Egapay.Customer.Business.Gateway.Common.Utils;
using Egapay.Customer.Business.Gateway.Common.Utils.Constants;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using Egapay.Customer.Business.Gateway.Common.Utils.Routes;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Services;

public class OnboardingAuthService
{
    private readonly ILogger<OnboardingAuthService> _logger;
    private readonly ApiClientService _client;
    private readonly InternalServicesConfig _internalServicesConfig;

    public OnboardingAuthService(ILogger<OnboardingAuthService> logger, ApiClientService client,
        IOptions<InternalServicesConfig> internalServicesConfig)
    {
        _logger = logger;
        _client = client;
        _internalServicesConfig = internalServicesConfig.Value;
    }
    
    public async Task<OperationResult<ApiClientResponse?>> RequestPasswordResetAsync(RequestPasswordResetRequest request,RequestHeaders headers,
        CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.RequestPasswordResetEndpoint);
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
            _logger.LogError($"{nameof(RequestPasswordResetAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(RequestPasswordResetAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }

    public async Task<OperationResult<ApiClientResponse?>> LoginMerchantAsync(LoginMerchantRequest request,RequestHeaders headers,
        CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.LoginEndpoint);
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
            _logger.LogError($"{nameof(LoginMerchantAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(LoginMerchantAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> ResetMerchantPasswordAsync(PasswordResetRequest request,RequestHeaders headers,
        CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.PasswordResetEndpoint);
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
            _logger.LogError($"{nameof(ResetPasswordRequest)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(ResetPasswordRequest)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
}
