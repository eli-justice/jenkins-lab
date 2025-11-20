using System.Text.RegularExpressions;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Utils;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Egapay.Customer.Business.Gateway.Common.Validations;

public class ValidatorBase<T> : AbstractValidator<T>, IDisposable
{
    private bool isDisposed = false;

    public async Task<ValidationResult> ValidateObjectAsync(T data)
    {
        var result = await ValidateAsync(data);
        return new ValidationResult()
        {
            IsValid = result.IsValid,
            Errors = Formatters.FormatValidationResults(result.Errors)
        };
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                // Free any other managed objects here.
                // e.g., release any managed resources or set large fields to null.
            }

            // Free any unmanaged resources here.
            isDisposed = true;
        }
    }
    
    protected bool MustBeValidPurpose(string purpose)
    {
        if (string.IsNullOrWhiteSpace(purpose) || !Enum.IsDefined(typeof(OtpGenerationPurpose), purpose))
        {
            return false;
        }
        return purpose != OtpGenerationPurpose.UNSPECIFIED.ToString();
    }
    
    protected bool BeValidEmailAddress(string? value)
    {
        return value is not null && Regex.IsMatch(value.ToLower(),@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
    } 
    protected bool BeAValidMobileNumber(string? value)
    {
        return value is not null && Regex.IsMatch(value,@"^\+?(\d{1,3})?[-.\s]?\(?\d{1,4}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$");
    }

    protected bool BeAValidOTP(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 6)
        {
            return false;
        }

        return Regex.IsMatch(value, @"^\d+$");
    }
    
    protected bool BeAValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
        return Regex.Match(password, "^[a-zA-Z0-9!@#$&:(),.\\-+%=]{8,}$").Success;
    } 
    
    protected bool BeAValidOtp(string otp)
    {
        if (string.IsNullOrWhiteSpace(otp))
        {
            return false;
        }
        return Regex.Match(otp, "^[0-9]{6}$").Success;
    }

    protected bool BeAValidDateString(string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
        {
            return false;
        }
        
        return Regex.Match(dateString,@"`^\d{4}-\d{2}-\d{2}$`").Success;
    }

    protected bool BeAValidDate(string? dateString)
    {
        try
        {
            if (dateString == null)
            {
                return false;
            }

            DateTime dateTimeValue;
            if (!DateTime.TryParse(dateString, out dateTimeValue))
            {
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    
    protected bool BeAValidDirectorPosition(string position)
    {
        if (string.IsNullOrWhiteSpace(position) || !Enum.IsDefined(typeof(DirectorPosition), position))
        {
            return false;
        }
        
        
        return position != DirectorPosition.UNKNOWN.ToString();
    }
    
    protected bool BeAValidCustomerIdType(string type)
    {
        if (string.IsNullOrWhiteSpace(type) || !Enum.IsDefined(typeof(CustomerIdType), type))
        {
            return false;
        }
        return type != CustomerIdType.UNKNOWN.ToString();
    }

    protected bool BeAValidUploadFile(IFormFile? file)
    {
        if (file == null || file.Length < 1)
        {
            return false;
        }

        return true;
    }
    
    protected bool BeWithinFileSizeLimit(IFormFile? file)
    {
        if (!BeAValidUploadFile(file))
        {
            return false;
        }

        if (file != null && file.Length > 1 * 1024 * 1024)
        {
            return false;
        }

        return true;
    }
    
    protected bool BeAValidTransactionType(string type)
    {
        if (string.IsNullOrWhiteSpace(type) || !Enum.IsDefined(typeof(TransactionType), type))
        {
            return false;
        }
        
        
        return type != TransactionType.UNKNOWN.ToString();
    }
    
    protected bool NotBeAFutureDate(string? dateString)
    {
        try
        {
            DateTime dateTimeValue;
            if (!DateTime.TryParse(dateString, out dateTimeValue))
            {
                return false;
            }
            
            return dateTimeValue <= DateTime.Now;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    
    protected bool NotBeAPastDate(string? dateString)
    {
        try
        {
            DateTime dateTimeValue;
            if (!DateTime.TryParse(dateString, out dateTimeValue))
            {
                return false;
            }
            
            return DateTime.Now <= dateTimeValue;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    protected bool BeAValidAmount(decimal amount)
    {
        return amount > 0;
    }
}