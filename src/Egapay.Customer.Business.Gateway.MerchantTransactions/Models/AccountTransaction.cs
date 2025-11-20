using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.MerchantTransactions.Models;

public class AccountTransactionRequest
{
    [JsonProperty("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;
    
    [JsonProperty("transactionType")]
    public string TransactionType { get; set; } = string.Empty;
    
    [JsonProperty("startDate")]
    public string StartDate { get; set; } = string.Empty;
    
    [JsonProperty("endDate")]
    public string EndDate { get; set; } = string.Empty;
    
    [JsonProperty("payPartnerServiceId")]
    public string PayPartnerServiceId { get; set; } = string.Empty;
    
    [JsonProperty("channel")]
    public string Channel { get; set; } = string.Empty;
}