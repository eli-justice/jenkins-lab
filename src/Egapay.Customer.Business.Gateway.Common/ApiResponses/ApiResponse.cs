using System.Text.Json;
using System.Text.Json.Serialization;

namespace Egapay.Customer.Business.Gateway.Common.ApiResponses;

public class ApiResponse<T>
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T Data { get; set; }

    public ApiResponse()
    {
        
    }
    public ApiResponse(string message, T data)
    {
        Message = message;
        Data = data;
    }
    public ApiResponse(int code, string message, T data)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    public ApiResponse(int code, string message)
    {
        Code = code;
        Message = message;
    }

}