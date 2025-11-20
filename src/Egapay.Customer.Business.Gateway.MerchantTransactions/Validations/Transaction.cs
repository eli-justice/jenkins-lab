
using Egapay.Customer.Business.Gateway.MerchantTransactions.Models;
using Egapay.Customer.Business.Gateway.Common.Resources;
using Egapay.Customer.Business.Gateway.Common.Validations;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Egapay.Customer.Business.Gateway.MerchantTransactions.Validations;

public class AccountTransactionRequestValidator : ValidatorBase<AccountTransactionRequest>
{
    public AccountTransactionRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x=>x.StartDate)
            .NotEmpty().WithMessage("startDate cannot be empty")
            .Must(BeAValidDate).WithMessage("startDate must be a valid date");
        
        RuleFor(x=>x.EndDate)
            .NotEmpty().WithMessage("endDate cannot be empty")
            .Must(BeAValidDate).WithMessage("endDate must be a valid date");
        
        RuleFor(x=>x.TransactionType)
            .NotEmpty().WithMessage("transactionType cannot be empty")
            .Must(BeAValidTransactionType).WithMessage("transactionType must be a valid date");
    }
}