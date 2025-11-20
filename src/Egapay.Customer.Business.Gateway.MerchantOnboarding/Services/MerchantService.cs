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

public partial class MerchantService
{
    private readonly ILogger<MerchantService> _logger;
    private readonly ApiClientService _client;
    private readonly InternalServicesConfig _internalServicesConfig;

    public MerchantService(ILogger<MerchantService> logger, ApiClientService client, IOptions<InternalServicesConfig> internalServicesConfig)
    {
        _logger = logger;
        _client = client;
        _internalServicesConfig = internalServicesConfig.Value;
    }
    
    public async Task<OperationResult<ApiClientResponse?>> MerchantAccountExistAsync(CheckMerchantRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.CheckMerchantAccountEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError($"{nameof(MerchantAccountExistAsync)} cancelled: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(MerchantAccountExistAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> RegisterMerchantAsync(CreateMerchantRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.RegisterMerchantEndpoint);
            
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(RegisterMerchantAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetMerchantBusinessInfoAsync(RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantBusinessInfoEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetMerchantBusinessInfoAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> UpdateMerchantBusinessInfoAsync(UpdateMerchantBusinessInfoRequest request, RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantBusinessInfoEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Put,headers,requestBody,token);
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateMerchantBusinessInfoAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> UpdateMerchantBusinessContactInfoAsync(UpdateBusinessContactRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantContactInfoEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Put,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateMerchantBusinessContactInfoAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetMerchantBusinessContactInfoAsync(RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"processing {nameof(GetMerchantBusinessContactInfoAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantContactInfoEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetMerchantBusinessContactInfoAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> AddContactPersonAsync(AddBusinessContactPersonRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantContactPersonEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddContactPersonAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> UpdateContactPersonAsync(UpdateBusinessContactPersonRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantContactPersonEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Put,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateContactPersonAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> DeleteContactPersonAsync(string contactId,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantOnboardingRoutes.MerchantContactPersonEndpoint}/{contactId}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Delete,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteContactPersonAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetAllContactPersonAsync(RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantContactPersonEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllContactPersonAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> AddDirectorOrShareholderAsync(AddBusinessDirectorOrShareholderRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantDirectorEndpoint);
            var requestBody = new RequestBody<MultipartContent>()
            {
                Type = RequestPayloadType.MULTIPART_FORM,
                Payload = Generators.BuildAsMultipartFormPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddDirectorOrShareholderAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> UpdateDirectorOrShareholderAsync(UpdateBusinessDirectorOrShareholderRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
 
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantDirectorEndpoint);
            var requestBody = new RequestBody<MultipartContent>()
            {
                Type = RequestPayloadType.MULTIPART_FORM,
                Payload = Generators.BuildAsMultipartFormPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Put,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateDirectorOrShareholderAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetAllDirectorOrShareholdersAsync(RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantDirectorEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllDirectorOrShareholdersAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> DeleteDirectorOrShareholdersAsync(string directorId,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantOnboardingRoutes.MerchantDirectorEndpoint}/{directorId}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Delete,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteDirectorOrShareholdersAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    
    public async Task<OperationResult<ApiClientResponse?>> AddDocumentAsync(AddBusinessDocumentRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"processing {nameof(AddDocumentAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantDocumentEndpoint);
            var requestBody = new RequestBody<MultipartContent>()
            {
                Type = RequestPayloadType.MULTIPART_FORM,
                Payload = Generators.BuildAsMultipartFormPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddDocumentAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    
    public async Task<OperationResult<ApiClientResponse?>> UpdateDocumentAsync(UpdateBusinessDocumentRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"processing {nameof(UpdateDocumentAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantDocumentEndpoint);
            var requestBody = new RequestBody<MultipartContent>()
            {
                Type = RequestPayloadType.MULTIPART_FORM,
                Payload = Generators.BuildAsMultipartFormPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Put,headers,requestBody,token);
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateDocumentAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> DeleteDocumentAsync(string documentId,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantOnboardingRoutes.MerchantDocumentEndpoint}/{documentId}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Delete,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(DeleteDocumentAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetAllDocumentsAsync(RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantDocumentEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllDocumentsAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetAllActiveRegulatorsAsync(RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.ActiveRegulatorsEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllActiveRegulatorsAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetAllActiveIndustriesAsync(RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();

            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.ActiveIndustriesEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllActiveIndustriesAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetAllMerchantServicesAsync(RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"actor processing {nameof(GetAllMerchantServicesAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals("MerchantTransactionsAPI"))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                MerchantOnboardingRoutes.MerchantServicesEndpoint);
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null,
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAllMerchantServicesAsync)}  exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> UpdateMerchantProfileImageAsync(UpdateMerchantProfileImageRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"processing {nameof(UpdateMerchantProfileImageAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantOnboardingRoutes.MerchantProfileEndpoint}/upload-image");
            var requestBody = new RequestBody<MultipartContent>()
            {
                Type = RequestPayloadType.MULTIPART_FORM,
                Payload = Generators.BuildAsMultipartFormPayload(request)
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(UpdateDocumentAsync)} exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetGeneratedApiKeyForMerchantAsync(string payPartnerService,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"actor processing {nameof(GetGeneratedApiKeyForMerchantAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantOnboardingRoutes.MerchantApiKeysEndpoint}?service={payPartnerService}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null,
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetGeneratedApiKeyForMerchantAsync)}  exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GenerateApiKeyForMerchantAsync(string service,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"actor processing {nameof(GenerateApiKeyForMerchantAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantOnboardingRoutes.MerchantApiKeysEndpoint}?service={service}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null,
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GenerateApiKeyForMerchantAsync)}  exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    
    public async Task<OperationResult<ApiClientResponse?>> GetCallbackUrlForMerchantAsync(string service,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"actor processing {nameof(GetCallbackUrlForMerchantAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantOnboardingRoutes.MerchantCallbackUrlEndpoint}?service={service}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = null,
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetCallbackUrlForMerchantAsync)}  exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> AddCallbackUrlForMerchantAsync(MerchantCallbackRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"actor processing {nameof(AddCallbackUrlForMerchantAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantOnboardingWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantOnboardingRoutes.MerchantCallbackUrlEndpoint}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request),
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(AddCallbackUrlForMerchantAsync)}  exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetUSSDMerchantCustomersAsync(string payPartnerService,string status,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"actor processing {nameof(GetUSSDMerchantCustomersAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.USSDMerchantCustomersUrlEndpoint}?payPartnerService={payPartnerService}&status={status}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetUSSDMerchantCustomersAsync)}  exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }

    public async Task<OperationResult<ApiClientResponse?>> ApproveUSSDMerchantCustomerAsync(ApproveUSSDMerchantCustomerRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"actor processing {nameof(GetUSSDMerchantCustomersAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.ApproveMerchantCustomerUrlEndpoint}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request),
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(ApproveUSSDMerchantCustomerAsync)}  exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
    
    public async Task<OperationResult<ApiClientResponse?>> GetUSSDMerchantCustomerPinResetRequestsAsync(string payPartnerService,string status,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"actor processing {nameof(GetUSSDMerchantCustomerPinResetRequestsAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.USSDMerchantCustomerPinResetUrlEndpoint}?payPartnerService={payPartnerService}&status={status}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Get,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetUSSDMerchantCustomersAsync)}  exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }

    public async Task<OperationResult<ApiClientResponse?>> ApproveUSSDMerchantCustomerPinResetAsync(ApproveUSSDMerchantCustomerPinResetRequest request,RequestHeaders headers,CancellationToken? token = null)
    {
        try
        {
            _logger.LogInformation($"actor processing {nameof(ApproveUSSDMerchantCustomerPinResetAsync)}");
            var appEnv = _internalServicesConfig.Services.Where(x => x.Name.Equals(InternalServiceNames.MerchantTransactionsWebApi))
                .Select(x=>x.Environment).FirstOrDefault();
            
            string url = Generators.BuildUrlForEganowServices(appEnv?.BaseUrl,
                $"{MerchantTransactionRoutes.ApproveMerchantCustomerPinResetUrlEndpoint}");
            var requestBody = new RequestBody<StringContent>()
            {
                Type = RequestPayloadType.JSON,
                Payload = Generators.BuildAsJsonPayload(request),
            };
            var response = await _client.MakeRequestAsync(url, HttpMethod.Post,headers,requestBody,token);
            
            return new OperationResult<ApiClientResponse?>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(ApproveUSSDMerchantCustomerAsync)}  exception: {ex.Message}");
            return new OperationResult<ApiClientResponse?>(new ErrorResult("failed to process the request. try again later"));
        }
    }
}