using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Egapay.Customer.Business.Gateway.Common.Utils;

public class Formatters
{
    public static Dictionary<string, List<string>> FormatValidationResults(ValidationResult validationResult)
    {
        var result = new Dictionary<string, List<string>>();

        var failedValidations = validationResult.Errors;
        if (failedValidations.Count  < 1)
        {
            return result;
        }
        
        var propertyNames = failedValidations.Select(x => x.PropertyName).ToList();

        foreach (var property in propertyNames)
        {
            var errors = failedValidations.Where(x => x.PropertyName == property).Select(x => x.ErrorMessage)
                .Distinct().ToList();
            
            if (!result.ContainsKey(JsonNamingPolicy.CamelCase.ConvertName(property)))
            {
                result.Add(JsonNamingPolicy.CamelCase.ConvertName(property), errors);
            }
        }
        
        return result;
    }
    
    public static Dictionary<string, object> FormatValidationResults(List<ValidationFailure> failedValidations)
    {
        var result = new Dictionary<string, object>();
        //get property names
        var propertyNames = failedValidations.Select(x => x.PropertyName).ToList();
        var tempFailedValidations = failedValidations;
        var nonClassValidations = tempFailedValidations.Where(x=>x.PropertyName.Contains(".") == false)
            .GroupBy(e => e.PropertyName.Split('.')[0])
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());

        if (nonClassValidations.Any())
        {
            foreach (var validation in nonClassValidations)
            {
                result.Add(JsonNamingPolicy.CamelCase.ConvertName(validation.Key), validation.Value);
            }

        }
        
        var classValidations = tempFailedValidations.Where(x => x.PropertyName.Contains("."))
            .GroupBy(e => e.PropertyName.Split('.')[0]).ToList();

        if (classValidations.Any())
        {
            foreach (var validation in classValidations)
            {
                var key = validation.Key;
                var dict = new Dictionary<string, List<string>>();
                var _propertyNames = validation.Select(x => x.PropertyName).Distinct().ToList();
            
                for (int i = 0; i < _propertyNames.Count; i++)
                {
                    var _propertyNameKey = _propertyNames[i].Split('.')[1];
                    var data = tempFailedValidations.Where(x => x.PropertyName == _propertyNames[i]).Select(x=>x.ErrorMessage).Distinct().ToList();
                    dict.Add(JsonNamingPolicy.CamelCase.ConvertName(_propertyNameKey), data);
                }
                result.Add(JsonNamingPolicy.CamelCase.ConvertName(key), dict);
            }
        }
        
        return result;
    }

    public static string FormatStatusForEganow(string status)
    {
        if (status.ToUpper() == "APPROVED")
        {
            return "SUCCESSFUL";
        }
        
        if (status.ToUpper() == "DECLINED")
        {
            return "FAILED";
        }

        return "PENDING";
    }

    public static NamingType GetJsonNamingPolicy(string json)
    {
        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        //if snake case
        if (dictionary.Keys.All(key => Regex.IsMatch(key, "^[a-z]+(_[a-z]+)*$")))
        {
            return NamingType.SnakeCase;
        }

        if (dictionary.Keys.All(key => Regex.IsMatch(key, "^[a-z]+[A-Za-z]*$") && !key.Contains("_")))
        {
            return NamingType.CamelCase;
        }

        return NamingType.Unknown;
    }
    
    public static int CalculateAmountForGatewayService(decimal amount)
    {
        // double tempAmount = amount + 0.06;
        // return Convert.ToInt32(tempAmount/0.01);
        return Convert.ToInt32(amount * 100);
    }

    public static string FormatStringNullValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "N/A";
        }
        
        return value;
    }

    public static string ConvertToCamelCase(string value)
    {
        return JsonNamingPolicy.CamelCase.ConvertName(value);
    }
    
    public static string FormatPhoneNumberWithDialCode(string phoneNumber, string dialCode)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(dialCode))
        {
            return phoneNumber;
        }

        string sanitizedDialCode = dialCode.Replace("+", "");

        if (phoneNumber.StartsWith(dialCode) || phoneNumber.StartsWith(sanitizedDialCode))
        {
            return phoneNumber.Replace("+", "");
        }

        return $"{dialCode}{phoneNumber.TrimStart('0')}".Replace("+", "");
    }

    public static string FormatStringToSnakeCase(string value)
    {
        try
        {
            string tempValue = value;
            
            // Remove non-alphanumeric characters except spaces
            tempValue = Regex.Replace(tempValue, @"[^\w\s]", "");

            // Replace spaces and uppercase letters with underscores
            tempValue = Regex.Replace(tempValue, @"\s+", "_"); // Replace spaces with underscores
            tempValue = Regex.Replace(tempValue, @"([a-z])([A-Z])", "$1_$2"); // Separate camel case

            // Convert to lowercase
            return tempValue.ToLowerInvariant();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return value;
        }
    }


    public static string ConvertStringToDate(string dateString)
    {
        try
        {
            DateTime date;

            if (!DateTime.TryParse(dateString, out date))
            {
                return dateString;
            }

            return date.ToString("yyyy-MM-dd");

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return dateString;
        }
    }
    
    public static byte[] ConvertIFormFileToByteArray(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return Array.Empty<byte>();

        using (var memoryStream = new MemoryStream())
        {
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}