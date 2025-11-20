using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;
namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class CheckMerchantAccountRequestValidator:ValidatorBase<CheckMerchantRequest>
{
    public CheckMerchantAccountRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x=>x.Email).
            NotEmpty().WithMessage(localizer["EmailNotEmpty"]).
            EmailAddress().WithMessage(localizer["InvalidEmail"]);
    }
}