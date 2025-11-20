using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Utils.Constants;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class UpdateMerchantBusinessInfoRequestValidator:ValidatorBase<UpdateMerchantBusinessInfoRequest>
{
    public UpdateMerchantBusinessInfoRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage(localizer["CompanyNameNotEmpty"]);

        RuleFor(x => x.TradingName)
            .NotEmpty().WithMessage(localizer["TradingNameNotEmpty"]);

        RuleFor(x => x.DateOfIncorporation)
            .NotEmpty().WithMessage(localizer["DateOfInCorporationNotEmpty"])
            .Must(BeAValidDate).WithMessage(localizer["DateOfInCorporationInvalid"])
            .Must(NotBeAFutureDate).WithMessage("date of corporation must not be ahead of current date");
        
        RuleFor(x=>x.CompanyRegistrationNo)
            .NotEmpty().WithMessage(localizer["CompanyRegistrationNoNotEmpty"]);
        
        RuleFor(x=>x.CompanyRegistrationType)
            .NotEmpty().WithMessage(localizer["CompanyRegistrationTypeNotEmpty"])
            .Must(BeValidCompanyRegistrationType).WithMessage(localizer["CompanyRegistrationTypeNotSpecified"]);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["EmailNotEmpty"])
            .Must(BeValidEmailAddress).WithMessage(localizer["InvalidEmail"]);

        RuleFor(x => x.RegulatorId)
            .NotEmpty().WithMessage("regulatorId cannot be empty.");
            //.Must(x=>BeRegulatedCompany(x,regulators)).WithMessage("regulatorId must be a regulated company.");
        
        RuleFor(x=>x.LicenseInfo.LicenseNumber)
            .NotEmpty().Unless(x=>x.RegulatorId.ToUpper().Equals(Constants.DefaultNotRegulatedValue)).WithMessage("license number cannot be empty");

        RuleFor(x => x.LicenseInfo.IssuedDate)
            .NotEmpty().Unless(x => x.RegulatorId.ToUpper().Equals(Constants.DefaultNotRegulatedValue)).WithMessage(localizer["IssueDateNotEmpty"])
            .Must(NotBeAFutureDate).Unless(x => x.RegulatorId.ToUpper().Equals(Constants.DefaultNotRegulatedValue)).WithMessage("issued date must not be ahead of current date");
            //.Must(x=>BeAValidDateIfBusinessIsNonRegulated(x)).WithMessage(localizer["IssueDateInvalid"]);

            RuleFor(x => x.LicenseInfo.ExpiryDate)
                .NotEmpty().Unless(x => x.RegulatorId.ToUpper().Equals(Constants.DefaultNotRegulatedValue))
                .WithMessage(localizer["ExpiryDateNotEmpty"])
                .Must(NotBeAPastDate).Unless(x => x.RegulatorId.ToUpper().Equals(Constants.DefaultNotRegulatedValue)).WithMessage("expiry date must not be behind current date");
            //.Must(BeAValidDateIfBusinessIsNonRegulated).WithMessage(localizer["InvalidExpiryDate"]);
        
        RuleFor(x=>x.TaxIdentificationNumber)
            .NotEmpty().WithMessage(localizer["TaxIDNumberNotEmpty"]);

        // RuleFor(x=>x.VATNumber)
        //     .NotEmpty().WithMessage(localizer["VATNumberNotEmpty"]);
    }

    protected bool BeValidCompanyRegistrationType(string registrationType)
    {
        
        if (string.IsNullOrWhiteSpace(registrationType) || !Enum.IsDefined(typeof(CompanyRegistrationType), registrationType))
        {
            return false;
        }
        return registrationType != CompanyRegistrationType.UNKNOWN.ToString();
    }
    
}