using System.Text.RegularExpressions;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using Microsoft.AspNetCore.Http;
using MimeDetective;

namespace Egapay.Customer.Business.Gateway.Common.Utils;

public class UtilHelpers
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }
        return Regex.Match(email.ToLower(), "^[a-z0-9._%+\\-]+@[a-z0-9.\\-]+\\.[a-z]{2,}$").Success;
    }
    
    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
        return Regex.Match(password, "^[a-zA-Z0-9!@#$&()\\-.+%=]{8,}$").Success;
    } 
    
    public static bool IsValidOtp(string otp)
    {
        if (string.IsNullOrWhiteSpace(otp))
        {
            return false;
        }
        return Regex.Match(otp, "^[0-9]{6}$").Success;
    }

    
    public static string GetOtpEmailContentTemplate()
    {
        return @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Eganow</title>
</head>
<body>
<table style=""max-width: 600px; margin: 0 auto; padding: 20px; font-family: Arial, sans-serif;"">
    <tr>
        <td style=""text-align: center;"">
            <img src=""https://res.cloudinary.com/eganow/image/upload/v1679061714/eganowservicegrouplogos/eganow.png"" alt=""Company Logo"" style=""max-width: 150px;"">
        </td>
    </tr>
    <tr>
        <td style=""padding: 40px 0; text-align: center; font-size: 24px;"">
            Your One-Time Password (OTP):
            <br>
            <strong style=""font-size: 36px; color: #007bff;"">{0}</strong>
        </td>
    </tr>
    <tr>
        <td style=""text-align: center;"">
            <p style=""font-size: 16px;"">This OTP is valid for {1} minutes.</p>
        </td>
    </tr>
    <tr>
        <td style=""text-align: center;"">
            <p style=""font-size: 14px;"">Please do not share this OTP with anyone for security reasons.</p>
        </td>
    </tr>
</table>
</body>
</html>
	";
    }

    public static CustomerStatus GetCustomerStatus(string status)
    {
        if (status == CustomerStatus.Pending.ToString())
        {
            return CustomerStatus.Pending;
        }
        
        if (status == CustomerStatus.Active.ToString())
        {
            return CustomerStatus.Active;
        }
        
        return CustomerStatus.Unknown;
    }

    public static string GetPasswordResetContentTemplate()
    {
        return @"<!DOCTYPE html>
	<html lang=""en"">
	<head>
	    <meta charset=""UTF-8"">
	    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
	    <title>Eganow</title>
	</head>
	<body>
	    <table style=""max-width: 600px; margin: 0 auto; padding: 20px; font-family: Arial, sans-serif;"">
	        <tr>
	            <td style=""text-align: center;"">
	                <img src=""https://res.cloudinary.com/eganow/image/upload/v1679061714/eganowservicegrouplogos/eganow.png"" alt=""Company Logo"" style=""max-width: 150px;"">
	            </td>
	        </tr>
	        <tr>
	            <td style=""padding: 40px 0; text-align: center; font-size: 24px;"">
	                Your Temporary Password:
	                <br>
	                <strong style=""font-size: 36px; color: #007bff;"">{0}</strong>
	            </td>
	        </tr>
	        <tr>
	            <td style=""text-align: center;"">
	                <p style=""font-size: 16px;"">This temporary password is valid for 24 hours.</p>
	            </td>
	        </tr>
	        <tr>
	            <td style=""text-align: center;"">
	                <p style=""font-size: 14px;"">Please change your password after logging in.</p>
	            </td>
	        </tr>
	    </table>
	</body>
	</html>";
    }


    public static bool HasClassProperty<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        // Get all properties of the object's type
        var properties = obj.GetType().GetProperties();

        // Check if any property is of class type
        return properties.Any(p => p.PropertyType.IsClass && p.PropertyType != typeof(string));
    }

    // public static List<object> GetProperties(object obj)
    // {
    //     // Get all properties of the object's type
    //     var properties = obj.GetType().GetProperties();
    //     var classList = properties.Where(p => p.PropertyType.IsClass).ToList();
    // }
    
    public static Dictionary<string, object> GetClassPropertiesAndValues(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var classProperties = new Dictionary<string, object>();

        // Get all properties of the object's type
        var properties = obj.GetType().GetProperties();

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;

            // Check if the property is a class and not a string
            if (propertyType.IsClass && propertyType != typeof(string))
            {
                // Get the value of the property
                var value = property.GetValue(obj);

                // Add to the result dictionary
                classProperties.Add(property.Name, value);
            }
        }

        return classProperties;
    }

    public static string? GetMimeTypeFromBase64String(string base64Image)
    {
        try
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);
            
            // Use MimeDetective to detect MIME type
            var inspector = new ContentInspectorBuilder() {
                Definitions = MimeDetective.Definitions.DefaultDefinitions.All()
            }.Build();
            
            var results = inspector.Inspect(imageBytes);
            return results.ByFileExtension().Select(x=>x.Extension).First();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    
    public static bool IsFileNullOrEmpty(IFormFile? file)
    {
        if (file is null) return true;

        return file.Length < 1;
    }
}