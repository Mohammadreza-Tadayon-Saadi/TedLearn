using Core.Securities;
using FluentValidation;
using Services.DTOs.Account;

namespace Services.DTOsValidator.Account;

public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordDto>
{
    public ForgetPasswordValidator()
    {
        RuleFor(x => x.PhoneNumber).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا شماره تلفن را وارد کنید.")
            .Matches(RegularExpression.PhoneRegularExpression()).WithMessage("شماره تلفن وارد شده صحیح نیست.")
            .Length(11).WithMessage("شماره تلفن وارد شده صحیح نیست.");
    }
}
