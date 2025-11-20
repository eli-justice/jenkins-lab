using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.MerchantTransactions.Models;

public class TransactionReportRequest
{
    [JsonProperty("startDate")]
    public string StartDate { get; set; } = string.Empty;
    [JsonProperty("endDate")]
    public string EndDate { get; set; } = string.Empty;
    [JsonProperty("service")]
    public string Service { get; set; } = string.Empty;

    public TransactionReportRequest(string startDate, string endDate, string service)
    {
        StartDate = startDate;
        EndDate = endDate;
        Service = service;
    }
}