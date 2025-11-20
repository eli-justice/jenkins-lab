using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class LoginMerchantRequestValidator : ValidatorBase<LoginMerchantRequest>
{
    public LoginMerchantRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x=>x.Email)
            .NotEmpty().WithMessage(localizer["EmailNotEmpty"])
            .Must(BeValidEmailAddress).WithMessage(localizer["InvalidEmail"]);
        
        RuleFor(x=>x.Password)
            .NotEmpty().WithMessage(localizer["PasswordNotEmpty"]);
    }
}

public class PasswordResetRequestValidator : ValidatorBase<PasswordResetRequest>
{
    public PasswordResetRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x=>x.Email)
            .NotEmpty().WithMessage(localizer["EmailNotEmpty"])
            .Must(BeValidEmailAddress).WithMessage(localizer["InvalidEmail"]);
        
        RuleFor(x=>x.Password)
            .NotEmpty().WithMessage(localizer["PasswordNotEmpty"]);
    }
}