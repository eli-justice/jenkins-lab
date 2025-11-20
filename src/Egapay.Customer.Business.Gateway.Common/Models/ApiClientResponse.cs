using System.Net;

namespace Egapay.Customer.Business.Gateway.Common.Models;

public class ApiClientResponse
{
    public HttpStatusCode Code { get; set; }
    public dynamic? Body { get; set; }
}