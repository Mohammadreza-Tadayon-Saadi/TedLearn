using FluentValidation;
using Services.DTOs.UserPanel;

namespace Services.DTOsValidator.UserPanel;

public class TransactionValidator : AbstractValidator<TransactionDto>
{
    public TransactionValidator()
    {
        RuleFor(t => t.Amount).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا مبلغ را وارد کنید.")
            .InclusiveBetween(1000 , 2500000).WithMessage("مبلغ فقط میتواند بین 1000 و 2500000 باشد.");
    }
}
