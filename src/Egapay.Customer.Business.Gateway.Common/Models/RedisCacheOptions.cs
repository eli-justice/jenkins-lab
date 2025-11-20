namespace Egapay.Customer.Business.Gateway.Common.Models;

public class RedisCacheOptions
{
    public int Index { get; set; } = 0;
    public TimeSpan? Expiry { get; set; } = TimeSpan.FromDays(1);

    public RedisCacheOptions(int index, TimeSpan? expiry)
    {
        Index = index;
        Expiry = expiry;
    }
    
    public RedisCacheOptions(int index)
    {
        Index = index;
    }
}