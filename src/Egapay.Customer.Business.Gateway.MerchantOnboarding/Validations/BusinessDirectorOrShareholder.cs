using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Utils.Enums;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class AddBusinessDirectorOrShareholderRequestValidator:ValidatorBase<AddBusinessDirectorOrShareholderRequest>
{
        public AddBusinessDirectorOrShareholderRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(localizer["FirstNameNotEmpty"]);
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(localizer["LastNameNotEmpty"]);
        
        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage(localizer["MobileNumberNotEmpty"]);   
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["EmailNotEmpty"])
            .Must(BeValidEmailAddress).WithMessage(localizer["InvalidEmail"]);
        
        RuleFor(x => x.Position)
            .NotEmpty().WithMessage(localizer["PositionNotEmpty"])
            .Must(BeAValidDirectorPosition).WithMessage(localizer["PositionNotSpecified"]);
        
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage(localizer["IdentificationNumNotEmpty"]);
        
        RuleFor(x => x.ExpiryDate)
            .NotEmpty().WithMessage(localizer["ExpiryDateNotEmpty"])
            .Must(BeAValidDate).WithMessage(localizer["InvalidExpiryDate"]);
        
        RuleFor(x => x.PlaceOfIssue)
            .NotEmpty().WithMessage(localizer["PlaceIssueNotEmpty"]);
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage(localizer["TypeNotEmpty"])
            .Must(BeAValidCustomerIdType).WithMessage(localizer["TypeNotSpecified"]);
        
        RuleFor(x => x.FrontImage)
            .NotEmpty().WithMessage(localizer["FrontImageNotEmpty"])
            .Must(BeWithinFileSizeLimit).WithMessage(localizer["DocumentSizeLimit"])
            .Must(BeAValidUploadFile).WithMessage(localizer["DocumentInvalid"]);;
        
        RuleFor(x => x.BackImage)
            .NotEmpty().WithMessage(localizer["BackImageNotEmpty"]).Unless(x=> x.Type.Equals(CustomerIdType.PASSPORT.ToString()))
            .Must(BeWithinFileSizeLimit).WithMessage(localizer["DocumentSizeLimit"]).Unless(x=>x.Type.Equals(CustomerIdType.PASSPORT.ToString()))
            .Must(BeAValidUploadFile).WithMessage(localizer["DocumentInvalid"]).Unless(x=> x.Type.Equals(CustomerIdType.PASSPORT.ToString()));

        RuleFor(x => x.PortraitImage)
            .NotEmpty().WithMessage(localizer["PortraitImageNotEmpty"])
            .Must(BeWithinFileSizeLimit).WithMessage(localizer["DocumentSizeLimit"])
            .Must(BeAValidUploadFile).WithMessage(localizer["DocumentInvalid"]);;
    }
}

public class UpdateBusinessDirectorOrShareholderRequestValidator:ValidatorBase<UpdateBusinessDirectorOrShareholderRequest>
{
    public UpdateBusinessDirectorOrShareholderRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.DirectorId)
            .NotEmpty().WithMessage("directorId cannot be empty");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(localizer["FirstNameNotEmpty"]);
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(localizer["LastNameNotEmpty"]);
        
        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage(localizer["MobileNumberNotEmpty"]);   
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["EmailNotEmpty"])
            .Must(BeValidEmailAddress).WithMessage(localizer["InvalidEmail"]);
        
        RuleFor(x => x.Position)
            .NotEmpty().WithMessage(localizer["PositionNotEmpty"])
            .Must(BeAValidDirectorPosition).WithMessage(localizer["PositionNotSpecified"]);
        
        
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage(localizer["IdentificationNumNotEmpty"]);
        
        RuleFor(x => x.ExpiryDate)
            .NotEmpty().WithMessage(localizer["ExpiryDateNotEmpty"])
            .Must(BeAValidDate).WithMessage(localizer["InvalidExpiryDate"])
            .Must(NotBeAPastDate).WithMessage("expiry date must not be behind the current date");
        
        RuleFor(x => x.PlaceOfIssue)
            .NotEmpty().WithMessage(localizer["PlaceIssueNotEmpty"]);
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage(localizer["TypeNotEmpty"])
            .Must(BeAValidCustomerIdType).WithMessage(localizer["TypeNotSpecified"]);
        
        // RuleFor(x => x.Identification.FrontImage)
        //     .NotEmpty().WithMessage(localizer["FrontImageNotEmpty"]);
        //
        // RuleFor(x => x.Identification.BackImage)
        //     .NotEmpty().WithMessage(localizer["BackImageNotEmpty"]);
        //
        // RuleFor(x => x.Identification.PortraitImage)
        //     .NotEmpty().WithMessage(localizer["PortraitImageNotEmpty"]);
    }
}