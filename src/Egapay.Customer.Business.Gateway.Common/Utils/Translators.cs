using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.Common.Utils;

public class Translators
{
    public static string GetLocalizedMessage(IStringLocalizer localizer,string key, string languageId)
    {
        var culture = new CultureInfo(languageId);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        return localizer[key];
    }
}