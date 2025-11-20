using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class UpdateBusinessContactRequestValidator:ValidatorBase<UpdateBusinessContactRequest>
{
    public UpdateBusinessContactRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.DigitalAddress)
            .NotEmpty().WithMessage(localizer["DigitalAddressNotEmpty"]);

        RuleFor(x => x.FirstOccupancyDate)
            .NotEmpty().WithMessage(localizer["FirstOccupancyDateNotEmpty"])            
            .Must(BeAValidDate).WithMessage("firstOccupancyDate must be a valid date.");

        RuleFor(x => x.OfficeOwnership)
            .NotEmpty().WithMessage(localizer["OfficeOwnershipNotEmpty"]);

        RuleFor(x => x.PostalAddress)
            .NotEmpty().WithMessage(localizer["PostalAddressNotEmpty"]);

        RuleFor(x => x.OfficeMobileNumber)
            .NotEmpty().WithMessage(localizer["MobileNumberNotEmpty"]);
    }

}