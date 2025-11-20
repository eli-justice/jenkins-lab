namespace Egapay.Customer.Business.Gateway.Common.Models;

public struct OperationResult<T>
{
    public T Value { get; set; }
    public ErrorResult? Error { get; set; }

    public OperationResult(T value, ErrorResult? error = null)
    {
        Value  = value;
        Error = error;
    }
    
    
    public OperationResult(ErrorResult? error)
    {
        Error = error;
    }
    
    public bool HasError => Error != null;
    public bool HasValue => Value != null;
    public T? GetValue()=> Value;
    public ErrorResult? GetError()=> Error;

    public Tuple<T?, ErrorResult?> ToTuple()
    {
        return new Tuple<T?, ErrorResult?>(Value, Error);
    }
}

public class ErrorResult
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ErrorResult()
    {
        
    }

    public ErrorResult(int code, string message)
    {
        StatusCode = code;
        Message = message;
    }
    
    public ErrorResult(string message)
    {
        StatusCode = 500;
        Message = message;
    }
}