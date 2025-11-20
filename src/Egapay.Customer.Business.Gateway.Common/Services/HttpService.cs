using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Egapay.Customer.Business.Gateway.Common.Services;

public class ApiClientService
{
    private readonly ILogger<ApiClientService> _logger;
    private ApplicationConfig _config;
    private readonly HttpClient _client;

    
    public ApiClientService(ILogger<ApiClientService> logger, IOptions<ApplicationConfig> config,IHttpClientFactory factory)
    {
        _logger = logger;
        _config = config.Value;
        _client = factory.CreateClient("GatewayHttpClient");

    }

    public async Task<ApiClientResponse?> MakeRequestAsync<TRequestBody>(string url,HttpMethod method, RequestHeaders? headers = null,
        RequestBody<TRequestBody>? body = null,CancellationToken? token = null) where TRequestBody : HttpContent
    {
        try
        {
            using var request = new HttpRequestMessage(method, new Uri(url))
            {
                Content = body?.Payload
            };

            _client.DefaultRequestHeaders.Accept.Clear();

            switch (body?.Type)
            {
                case RequestPayloadType.MULTIPART_FORM:
                    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    break;
                case RequestPayloadType.JSON:
                    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    break;
                default:
                    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    break;
            }
            
            if (headers != null)
            {
                if (headers.HasBearerToken)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", headers.BearerToken);
                }
                else if (headers.HasBasicAuth)
                {
                    var bytes = Encoding.ASCII.GetBytes($"{headers.BasicAuth.Username}:{headers.BasicAuth.Password}");
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
                }

                if (headers.HasRequiredParams)
                {
                    request.Headers.TryAddWithoutValidation("x-country-code", headers.CountryCode);
                    request.Headers.TryAddWithoutValidation("x-language-id", headers.LanguageId);
                    request.Headers.TryAddWithoutValidation("x-ega-user-access-token", headers.AccessToken);
                }
            }
            
            var response = await _client.SendAsync(request);

            if (response.StatusCode is HttpStatusCode.MethodNotAllowed or HttpStatusCode.NotFound or HttpStatusCode.UnsupportedMediaType)
            {
                return new ApiClientResponse
                {
                    Code = response.StatusCode,
                    Body = new { code = response.StatusCode, message = response.ReasonPhrase }
                };
            }

            var contentType = response.Content.Headers.ContentType?.MediaType;

            if (contentType == "application/json")
            {
                return new ApiClientResponse
                {
                    Code = response.StatusCode,
                    Body = await response.Content.ReadFromJsonAsync<dynamic?>()
                };
            }

            return new ApiClientResponse { Code = response.StatusCode }; // fallback
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(MakeRequestAsync)} exception: {ex.Message}");
            _logger.LogError($"{nameof(MakeRequestAsync)} exception stack trace: {ex.StackTrace ?? ex.InnerException?.StackTrace}");
            return new ApiClientResponse(){Code = HttpStatusCode.InternalServerError,Body = new {code=  500, message = "something went wrong. try again later."}};
        }
    }
    
}