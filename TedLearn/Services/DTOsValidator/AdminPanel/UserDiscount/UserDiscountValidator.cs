using Core.Securities;
using FluentValidation;
using Services.DTOs.AdminPanel.UserDiscount;

namespace Services.DTOsValidator.AdminPanel.UserDiscount;

public class UserDiscountValidator : AbstractValidator<UserDiscountDto>
{
    public UserDiscountValidator()
    {
        RuleFor(x => x.DiscountCode).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا کد تخفیف را وارد کنید.")
            .MaximumLength(80).WithMessage("کد تخفیف نباید بیشتر از 80 کارکتر باشد.");

        RuleFor(x => (int)x.Percent).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا درصد تخفیف را وارد کنید.")
            .InclusiveBetween(1, 100).WithMessage("درصد تخفیف فقط میتواند بین 1 و 100 باشد.");

        RuleFor(x => (int)x.UsableCount).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا تعداد تخفیف را وارد کنید.")
            .InclusiveBetween(1, 100).WithMessage("تعداد تخفیف فقط میتواند بین 1 و 100 باشد.");

        RuleFor(x => x.StartDateShamsi).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا تاریخ شروع را وارد کنید.")
            .Matches(RegularExpression.PersianDateRegularExpression(true)).WithMessage("لطفا تاریخ را به درستی وارد کنید.");

        RuleFor(x => x.EndDateShamsi).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا تاریخ اتمام را وارد کنید.")
            .Matches(RegularExpression.PersianDateRegularExpression(true)).WithMessage("لطفا تاریخ را به درستی وارد کنید.");
    }
}
