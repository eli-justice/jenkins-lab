using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Egapay.Customer.Business.Gateway.Common.Utils;

public class Generators
{
    public static StringContent BuildAsJsonPayload<TPayload>(TPayload payload)
    {
        return new StringContent(JsonSerializer.Serialize(payload,new JsonSerializerOptions(){PropertyNamingPolicy = JsonNamingPolicy.CamelCase}), Encoding.UTF8, "application/json");
    }
    
    public static MultipartContent BuildAsMultipartFormPayload<TPayload>(TPayload payload) where TPayload: class
    {
        //get properties
        var properties = payload?.GetType().GetProperties();
        var content = new MultipartFormDataContent();
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(IFormFile))
            {
                var formFile = (IFormFile)property.GetValue(payload);
                if (!UtilHelpers.IsFileNullOrEmpty(formFile))
                {
                    //var fileBytes = Formatters.ConvertIFormFileToByteArray(formFile);
                    var byteArrayContent = new ByteArrayContent(Formatters.ConvertIFormFileToByteArray(formFile));
                    byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType ?? "application/octet-stream");
                    content.Add(byteArrayContent,Formatters.ConvertToCamelCase(property.Name),formFile.FileName);
                }
            }
            if (property.PropertyType == typeof(string))
            {
                content.Add(new StringContent(property.GetValue(payload)?.ToString()), Formatters.ConvertToCamelCase(property.Name));
            }
        }

        return content;
    }

    public static string BuildUrlForEganowServices(string baseUrl, string endpoint, int serviceVersion = 1)
    {
        return $"{baseUrl.TrimEnd('/')}/api/v{serviceVersion}/{endpoint}";
    }
}