using FluentValidation;
using Services.DTOs.Account;

namespace Services.DTOsValidator.Account;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(u => u.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا نام کاربری را وارد کنید.");

        RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا رمز عبور را وارد کنید.");
    }
}
