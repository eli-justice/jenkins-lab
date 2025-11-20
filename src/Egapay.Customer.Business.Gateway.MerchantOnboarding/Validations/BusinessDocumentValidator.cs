using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class AddBusinessDocumentValidator:ValidatorBase<AddBusinessDocumentRequest>
{
    public AddBusinessDocumentValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x=>x.Name)
            .NotEmpty().WithMessage("name cannot be empty");
        
        RuleFor(x=>x.Document)
            .NotEmpty().WithMessage(localizer["DocumentNotEmpty"])
            .Must(BeWithinFileSizeLimit).WithMessage(localizer["DocumentSizeLimit"])
            .Must(BeAValidUploadFile).WithMessage(localizer["DocumentInvalid"]);
        
    }
}

public class UpdateBusinessDocumentValidator:ValidatorBase<UpdateBusinessDocumentRequest>
{
    public UpdateBusinessDocumentValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x=>x.Name)
            .NotEmpty().WithMessage("name cannot be empty");
        
        RuleFor(x=>x.DocumentId)
            .NotEmpty().WithMessage(localizer["DocumentIdNotEmpty"]);
        
        RuleFor(x=>x.Document)
            .NotEmpty().WithMessage(localizer["DocumentNotEmpty"])
            .Must(BeWithinFileSizeLimit).WithMessage(localizer["DocumentSizeLimit"])
            .Must(BeAValidUploadFile).WithMessage(localizer["DocumentInvalid"]);
        
    }
}