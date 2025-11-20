using Egapay.Customer.Business.Gateway.Common.Resources;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.Common.Services;

public class ServiceBase
{
    public IStringLocalizer<SuccessMessages> _successLocalizer;
    public IStringLocalizer<ErrorMessages> _errorLocalizer;
    public IStringLocalizer<ValidationMessages> _validationLocalizer;
    public string _culture;
    public ApiClientService _apiClientService;
    
    public void SetCulture(string languageId)
    {
        _culture = languageId;
    }
}