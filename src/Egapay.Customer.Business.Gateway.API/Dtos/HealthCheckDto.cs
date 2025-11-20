namespace Egapay.Customer.Business.Gateway.API.Features.Shared.Dtos;

public class HealthCheckDto
{
    public string Name { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string Duration { get; set; }
    public object? Exception { get; set; }
    public object Tags { get; set; }
}

public class GatewayHealthCheckDto
{
    public string Name { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public string Duration { get; set; }
}

