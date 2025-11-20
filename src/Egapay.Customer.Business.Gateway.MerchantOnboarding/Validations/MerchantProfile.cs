using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using Egapay.Customer.Business.Gateway.MerchantOnboarding.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantOnboarding.Validations;

public class UpdateMerchantProfileImageValidator:ValidatorBase<UpdateMerchantProfileImageRequest>
{
    public UpdateMerchantProfileImageValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x=>x.Name)
            .NotEmpty().WithMessage("name cannot be empty");
        
        RuleFor(x=>x.Image)
            .NotEmpty().WithMessage("image cannot be empty")
            .Must(BeWithinFileSizeLimit).WithMessage(localizer["DocumentSizeLimit"])
            .Must(BeAValidUploadFile).WithMessage(localizer["DocumentInvalid"]);
        
    }
}