using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class VerifyOtpRequestValidator:ValidatorBase<VerifyOtpRequest>
{
    public VerifyOtpRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x=>x.Email).
            NotEmpty().WithMessage(localizer["EmailNotEmpty"]).
            EmailAddress().WithMessage(localizer["InvalidEmail"]);

        RuleFor(x => x.Otp).
            NotEmpty().WithMessage(localizer["OtpNotEmpty"]);

        
        RuleFor(x=>x.Purpose).
            Must(MustBeValidPurpose).WithMessage(localizer["PurposeNotSpecified"]);
    }


}