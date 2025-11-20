using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Egapay.Customer.Business.Gateway.API.Interfaces;
using Egapay.Customer.Business.Gateway.Common.Configs;
using Egapay.Customer.Business.Gateway.Common.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Services;
using Egapay.Customer.Business.Gateway.Common.Utils;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Egapay.Customer.Business.Gateway.API.Services;

public class AuthService:ServiceBase,IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly ApplicationConfig _applicationConfig;
    private readonly IAuthRedisRepository _authRedisRepository;
    private readonly string _accessTokenKeyPrefix = "access_tokens";

    public AuthService(ILogger<AuthService> logger,IOptions<ApplicationConfig> applicationConfig,
    IAuthRedisRepository authRedisRepository,IStringLocalizer<ErrorMessages> errorMessages)
    {
        _logger = logger;
        _applicationConfig = applicationConfig.Value;
        _authRedisRepository = authRedisRepository;
        _errorLocalizer = errorMessages;
    }
    public async  Task<ClaimsPrincipal?> ValidateTokenAsync(string token, string secret,bool validateLifeTime = false)
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
            ValidateLifetime = validateLifeTime, // Check token expiration
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
                return await Task.FromResult(principal); // The token is valid
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(ValidateTokenAsync)} exception: {ex.Message}");
        }

        return await Task.FromResult(new ClaimsPrincipal());
    }

    public async Task<MerchantClaims?> GetAuthUserClaimsAsync(string token)
    {
        try
        {
            //check if token exists in cache
            string key = $"{_accessTokenKeyPrefix}:{token}";
            var cacheResult = await _authRedisRepository.GetAuthTokenAsync(key);
            if (cacheResult.HasError)
            {
                _logger.LogError($"{nameof(GetAuthUserClaimsAsync)} failed to get access token: {cacheResult.GetError()}");
                return null;
            }
            //if it does not exists return null
            if (!cacheResult.HasValue || string.IsNullOrWhiteSpace(cacheResult.GetValue()))
            {
                _logger.LogError($"{nameof(GetAuthUserClaimsAsync)}: token does not exist in cache");
                return null;
            }
        
            //if exists, get principal claims from token
            //map to claims to auth user claims
            var claims =  Tokenizers.MapPrincipalClaimsToMerchantClaims(Tokenizers.ValidateToken(token, _applicationConfig.JwtSecretKey,false));
            if (claims == null)
            {
                return null;
            }

            //cache or reset timer
            _authRedisRepository.SaveAuthTokenAsync(key, token);
            return claims;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(GetAuthUserClaimsAsync)} exception: {ex.Message}");
            return null;
        }
    }
    
    public void SetCulture(string languageId)
    {
        _culture = languageId;
    }
}