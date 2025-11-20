using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Egapay.Customer.Business.Gateway.Common.Resources;

public class EganowRequestCultureProvider: RequestCultureProvider
{
    private string UserLanguage { get; set; }
    
    public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        await Task.Yield();
        if (string.IsNullOrWhiteSpace(UserLanguage))
        {
            return new ProviderCultureResult("en", "en");
        }
        else if (UserLanguage != "en" && UserLanguage != "fr")
        {
            return new ProviderCultureResult("en", "en");
        }
        else
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(request.LanguageId);
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(UserLanguage);
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(UserLanguage);
            return new ProviderCultureResult(UserLanguage, UserLanguage);
        }

    }
    
    public async Task SetUserLanguage(string userLanguageSelected)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userLanguageSelected))
            {
                UserLanguage = "en";
            }
            else
            {
                UserLanguage = userLanguageSelected;
            }

            await DetermineProviderCultureResult(null);
        }
        catch (Exception ex)
        {
            string sss = ex.Message;

        }
    }
}