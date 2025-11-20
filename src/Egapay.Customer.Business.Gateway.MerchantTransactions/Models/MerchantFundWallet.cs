using Newtonsoft.Json;

namespace Egapay.Customer.Business.Gateway.MerchantTransactions.Models;

public class MerchantFundWalletRequest
{
    [JsonProperty("payPartnerServiceId")]
    public string PayPartnerServiceId { get; set; } = string.Empty;
    
    [JsonProperty("transactionType")]
    public string TransactionType { get; set; } = string.Empty;
    
    [JsonProperty("amount")]
    public decimal Amount { get; set; }
    
    [JsonProperty("narration")]
    public string Narration { get; set; } = string.Empty;
}

public class GetMerchantFundWalletRequest
{
    [JsonProperty("payPartnerServiceId")]
    public string PayPartnerServiceId { get; set; } = string.Empty;
    
    [JsonProperty("transactionType")]
    public string TransactionType { get; set; } = string.Empty;
    
    [JsonProperty("startDate")]
    public string StartDate { get; set; } = string.Empty;
    
    [JsonProperty("endDate")]
    public string EndDate { get; set; } = string.Empty;
    
}