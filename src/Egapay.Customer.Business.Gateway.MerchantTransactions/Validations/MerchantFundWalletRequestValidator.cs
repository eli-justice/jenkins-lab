using Egapay.Customer.Business.Gateway.MerchantTransactions.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantTransactions.Validations;

public class MerchantFundWalletRequestValidator:ValidatorBase<MerchantFundWalletRequest>
{
    public MerchantFundWalletRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.PayPartnerServiceId)
            .NotEmpty().WithMessage("payPartnerServiceId cannot be empty");
        
        RuleFor(x=>x.TransactionType)
            .NotEmpty().WithMessage("transactionType cannot be empty");
        RuleFor(x=>x.Amount)
            .NotEmpty().WithMessage("amount cannot be empty")
            .Must(BeAValidAmount).WithMessage("amount must be a valid date");
    }
    
}

public class GetMerchantFundWalletRequestValidator:ValidatorBase<GetMerchantFundWalletRequest>
{
    public GetMerchantFundWalletRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.PayPartnerServiceId)
            .NotEmpty().WithMessage("payPartnerServiceId cannot be empty");
        
        RuleFor(x=>x.TransactionType)
            .NotEmpty().WithMessage("transactionType cannot be empty");
        
        RuleFor(x=>x.StartDate)
            .NotEmpty().WithMessage("startDate cannot be empty")
            .Must(BeAValidDate).WithMessage("startDate must be a valid date");
        
        RuleFor(x=>x.EndDate)
            .NotEmpty().WithMessage("endDate cannot be empty")
            .Must(BeAValidDate).WithMessage("endDate must be a valid date");
    }
    
}