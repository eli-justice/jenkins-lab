using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Egapay.Customer.Business.Gateway.Common.Models;
using Microsoft.IdentityModel.Tokens;

namespace Egapay.Customer.Business.Gateway.Common.Utils;

public class Tokenizers
{
    public static ClaimsPrincipal? ValidateToken(string token, string secret,bool validateLifeTime = true)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // Create the validation parameters
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), // Use the same key as in token generation
            ValidateIssuer = true,
            ValidIssuer = "Eganow", // Replace with your actual issuer
            ValidateAudience = false, // Change this to true if you want to validate audience
            ValidateLifetime = false, // Check token expiration
            ClockSkew = TimeSpan.Zero // Optional: no clock skew tolerance
        };

        try
        {
            // Validate the token and return the principal
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // Ensure the token is a valid JWT
            if (validatedToken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                return principal; // The token is valid
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
        }

        return null; // Token is invalid
    }

    public static MerchantClaims? MapPrincipalClaimsToMerchantClaims(ClaimsPrincipal? principal)
    {
        if (principal == null)
        {
            return null;
        }


        return new MerchantClaims()
        {
            CustomerId = principal.Claims.Where(x=>x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Select(x=>x.Value).FirstOrDefault(),
            Email = principal.Claims.Where(x=>x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Select(x=>x.Value).FirstOrDefault(),
            CustomerGuid = principal.Claims.Where(x=>x.Type == "SerialNumber").Select(x=>x.Value).FirstOrDefault(),
            Mobile = principal.Claims.Where(x=>x.Type == "MobilePhone").Select(x=>x.Value).FirstOrDefault(),
            FirstName = principal.Claims.Where(x=>x.Type == "GivenName").Select(x=>x.Value).FirstOrDefault(),
            LastName = principal.Claims.Where(x=>x.Type == "Surname").Select(x=>x.Value).FirstOrDefault(),
        };
    }
}