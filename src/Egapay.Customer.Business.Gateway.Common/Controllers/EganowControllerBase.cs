using System.Text;
using Egapay.Customer.Business.Gateway.Common.ApiResponses;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Extensions;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.Common.Controllers;

public class EganowControllerBase:ControllerBase
{
    public ApplicationConfig _config;
    public IStringLocalizer<SuccessMessages> _successLocalizer;
    public IStringLocalizer<ErrorMessages> _errorLocalizer;
    public IStringLocalizer<ValidationMessages> _validationLocalizer;
    List<string> keys = new List<string>(){"x-country-code","x-language-id","x-ega-user-access-token"};
    public static string MerchantId,MerchantSecret;
    
        public RequestHeaders? GetRequestHeaders(IHeaderDictionary? headers = null)
    {
        try
        {
            var tempHeaders = new List<string>();
            if (headers == null)
            {
                headers = Request.Headers;

            }


            var controllerHeaders = headers.ToDictionary();
            if (!controllerHeaders.Any())
            {
                return null;
            }
            for (int i = 0; i < keys.Count; i++)
            {
                tempHeaders.Add(controllerHeaders.FirstOrDefault(x => x.Key.Equals(keys[i])).Value.ToString());
            }

        
            return new RequestHeaders()
            {
                CountryCode = tempHeaders[0],
                LanguageId = tempHeaders[1],
                AccessToken = tempHeaders[2],
                BasicAuth = GetBasicAuthentication(headers),
                BearerToken = GetBearerToken(headers),
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
        
    }

    BasicAuthentication? GetBasicAuthentication(IHeaderDictionary headers)
    {

        try
        {
            // Check if the Authorization header is present
            if (headers.TryGetValue("Authorization", out var authHeader))
            {
                // Ensure it's a Basic Auth header
                if (authHeader.ToString().StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    // Decode the Base64 encoded username:password part
                    var encodedCredentials = authHeader.ToString().Substring("Basic ".Length).Trim();
                    var decodedBytes = Convert.FromBase64String(encodedCredentials);
                    var credentials = Encoding.UTF8.GetString(decodedBytes);

                    // Split into username and password
                    var parts = credentials.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        var username = parts[0];
                        var password = parts[1];

                        if (!username.Equals(MerchantId) || !password.Equals(MerchantSecret))
                        {
                            return null;
                        }
                        return new BasicAuthentication()
                        {
                            Username = username,
                            Password = password
                        };
                    }
                }
            }
        
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    
    string? GetBearerToken(IHeaderDictionary headers)
    {

        try
        {
            // Check if the Authorization header is present
            if (headers.TryGetValue("Authorization", out var authHeader))
            {
                // Ensure it's a Basic Auth header
                if (authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    // Decode the Base64 encoded username:password part
                    var credentials = authHeader.ToString().Substring("Bearer ".Length).Trim();

                    if (string.IsNullOrWhiteSpace(credentials))
                    {
                        return null;
                    }
                    
                    return credentials;
                }
            }
        
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    
    
    /// <summary>
    /// check if this error is a failed result
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool IsFailedResponse(ErrorResult? result)
    {
        if (result == null)
        {
            return false;
        }
        return result.IsBadRequestError() || result.IsInternalServerError() || result.StatusCode > 200;
    }

    /// <summary>
    /// this returns failed responses only
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IActionResult SetFailedResponse(ErrorResult? result)
    {
        if (result.IsBadRequestError())
        {
            return StatusCode(result.StatusCode,new BadRequestResponse<string>(result.Message));
        }
        
        return StatusCode(result.StatusCode,new ApiResponse<string>(result.StatusCode,result.Message));
        
        
    }
}