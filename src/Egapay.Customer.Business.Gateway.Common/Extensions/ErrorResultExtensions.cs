

using Egapay.Customer.Business.Gateway.Common.Models;

namespace Egapay.Customer.Business.Gateway.Common.Extensions;

public static class ErrorResultExtensions
{
    public static bool IsBadRequestError(this ErrorResult? result)
    {
        return result != null && result.StatusCode == 400;
    }
    
    public static bool IsInternalServerError(this ErrorResult? result)
    {
        return result != null && result.StatusCode == 500;
    }

    public static ErrorResult ToErrorResult(this string str)
    {
        return new ErrorResult(str);
    }
}