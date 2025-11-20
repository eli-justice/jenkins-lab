namespace Egapay.Customer.Business.Gateway.Common.Models;

public class PaginationOption
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }

    public PaginationOption()
    {
        
    }

    public PaginationOption(int page,int limit)
    {
        Page = page;
        Limit = limit;
        Offset = (page - 1) * limit;
    }

    public string AppendPaginationQueryString(string query)
    {
        return $"{query} limit {Limit} offset {Offset}";
    }
}