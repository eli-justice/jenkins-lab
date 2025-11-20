using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.Common.ApiResponses;

public class BadRequestResponse<T>
{
    [JsonProperty("code")]
    public int Code { get; set; } = 400;
    [JsonProperty("message")]
    public string Message { get; set; }
    [JsonProperty("errors",NullValueHandling=NullValueHandling.Ignore)]
    public T Errors { get; set; }

    public BadRequestResponse()
    {
        
    }
    public BadRequestResponse(string message, T data)
    {
        Message = message;
        Errors = data;
    }
    public BadRequestResponse(string message)
    {
        Message = message;
    }

}