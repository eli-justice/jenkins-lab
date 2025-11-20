using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;
namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class CreateMerchantRequestValidator:ValidatorBase<CreateMerchantRequest>
{
    public CreateMerchantRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        //personal information
        RuleFor(x => x.PersonalInformation.FirstName)
            .NotEmpty().WithMessage(localizer["FirstNameNotEmpty"]);
        
        RuleFor(x => x.PersonalInformation.LastName)
            .NotEmpty().WithMessage(localizer["LastNameNotEmpty"]);
        
        RuleFor(x => x.PersonalInformation.Email)
            .NotEmpty().WithMessage(localizer["EmailNotEmpty"])
            .Must(BeValidEmailAddress).WithMessage(localizer["InvalidEmail"]);

        RuleFor(x => x.BusinessInformation.MobileNumber)
            .NotEmpty().WithMessage(localizer["MobileNumberNotEmpty"])
            .MinimumLength(10).WithMessage(localizer["MobileNumberLeast"])
            .Must(BeAValidMobileNumber).WithMessage(localizer["InvalidMobileNumber"]);
        
        //=============================================================
        //business information
        RuleFor(x=>x.BusinessInformation.BusinessName).
            NotEmpty().WithMessage(localizer["BusinessNameNotEmpty"]).
            MinimumLength(5).WithMessage(localizer["BusinessNameLeast"]);

        RuleFor(x => x.BusinessInformation.TradingName).
            NotEmpty().WithMessage(localizer["TradingNameNotEmpty"]).
            MinimumLength(5).WithMessage(localizer["TradingNameLeast"]);

        RuleFor(x => x.BusinessInformation.MobileNumber)
            .NotEmpty().WithMessage(localizer["MobileNumberNotEmpty"])
            .MinimumLength(10).WithMessage(localizer["MobileNumberLeast"])
            .Must(BeAValidMobileNumber).WithMessage(localizer["InvalidMobileNumber"]);
        
        RuleFor(x => x.Password).
            NotEmpty().WithMessage(localizer["PasswordNotEmpty"])
            .MinimumLength(8).WithMessage(localizer["PasswordLeast"])
            .Must(BeAValidPassword).WithMessage(localizer["PasswordInvalid"]);
        
        RuleFor(x => x.Otp).
            NotEmpty().WithMessage(localizer["OtpNotEmpty"])
            .Must(BeAValidOTP).WithMessage(localizer["OtpInvalid"]);
    }
}