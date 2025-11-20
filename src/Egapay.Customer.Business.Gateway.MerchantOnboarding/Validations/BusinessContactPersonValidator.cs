using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class AddBusinessContactPersonRequestValidator:ValidatorBase<AddBusinessContactPersonRequest>
{
    public AddBusinessContactPersonRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(localizer["FirstNameNotEmpty"]);
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(localizer["LastNameNotEmpty"]);
        
        RuleFor(x=>x.Email).
            NotEmpty().WithMessage(localizer["EmailNotEmpty"]).
            EmailAddress().WithMessage(localizer["InvalidEmail"]);
        
        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage(localizer["MobileNumberNotEmpty"])
            .Must(BeAValidMobileNumber).WithMessage(localizer["InvalidMobileNumber"]);
        
        RuleFor(x => x.Position)
            .NotEmpty().WithMessage(localizer["PositionNotEmpty"])
            .Must(BeAValidDirectorPosition).WithMessage(localizer["PositionNotSpecified"]);
    }
}

public class UpdateBusinessContactPersonRequestValidator:ValidatorBase<UpdateBusinessContactPersonRequest>
{
    public UpdateBusinessContactPersonRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.ContactId)
            .NotEmpty().WithMessage(localizer["ContactIdNotEmpty"]);
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(localizer["FirstNameNotEmpty"]);
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(localizer["LastNameNotEmpty"]);
        
        RuleFor(x=>x.Email).
            NotEmpty().WithMessage(localizer["EmailNotEmpty"]).
            EmailAddress().WithMessage(localizer["InvalidEmail"]);
        
        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage(localizer["MobileNumberNotEmpty"])
            .Must(BeAValidMobileNumber).WithMessage(localizer["InvalidMobileNumber"]);
        
        RuleFor(x => x.Position)
            .NotEmpty().WithMessage(localizer["PositionNotEmpty"])
            .Must(BeAValidDirectorPosition).WithMessage(localizer["PositionNotSpecified"]);
    }


}