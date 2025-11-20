namespace Egapay.Customer.Business.Gateway.Common.Utils;

public class HttpCodesHelpers
{
    public static bool IsSuccessCode(int code)
    {
        return code >= 200 && code <= 299;
    }
    
    public static bool IsBadRequest(int code)
    {
        return code == 400 || code == 422;
    }
    
    public static bool IsServerErrorCode(int code)
    {
        return code == 500 || code <= 599;
    }
}