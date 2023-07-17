using Core.Securities;
using FluentValidation;
using Services.DTOs.AdminPanel.User;

namespace Services.DTOsValidator.AdminPanel.User;

public class AddUserValidator : AbstractValidator<AddUserDto>
{
    public AddUserValidator()
    {
        RuleFor(u => u.UserName).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا نام کاربری را وارد کنید.")
            .MinimumLength(3).WithMessage("نام کاربری باید بیشتر از 3 کارکتر باشد.")
            .MaximumLength(80).WithMessage("نام کاربری نباید بیشتر از 80 کارکتر باشد.");

        RuleFor(x => x.PhoneNumber).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا شماره تلفن را وارد کنید.")
            .Matches(RegularExpression.PhoneRegularExpression()).WithMessage("شماره تلفن وارد شده صحیح نیست.")
            .Length(11).WithMessage("شماره تلفن وارد شده صحیح نیست.");

        var passwordValidationInfo = RegularExpression.GetPasswordValidationPatternAndMessage();
        RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا رمز عبور را وارد کنید.")
            .Matches(passwordValidationInfo.Pattern).WithMessage(passwordValidationInfo.ErrorMessage)
            .MinimumLength(4).WithMessage("رمز عبور باید بیشتر از 4 کارکتر باشد.")
            .MaximumLength(40).WithMessage("رمز عبور نباید بیشتر از 44 باشد.");

        RuleFor(x => x.RePassword).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا تکرار رمز عبور را وارد کنید.")
            .Equal(x => x.Password).WithMessage("تکرار رمز عبور با رمز عبور وارد شده همخوانی ندارد.");
    }
}
