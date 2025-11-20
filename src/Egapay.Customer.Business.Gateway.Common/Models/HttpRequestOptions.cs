namespace Egapay.Customer.Business.Gateway.Common.Models;

public class HttpRequestOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public HttpMethod ActionVerb { get; set; } = HttpMethod.Get;
    public RequestHeaders Headers { get; set; } = new RequestHeaders();
}