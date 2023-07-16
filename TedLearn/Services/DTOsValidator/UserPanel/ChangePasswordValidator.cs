using Core.Securities;
using FluentValidation;
using Services.DTOs.UserPanel;

namespace Services.DTOsValidator.UserPanel;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("لطفا رمز عبور را وارد کنید.");

        var passwordValidationInfo = RegularExpression.GetPasswordValidationPatternAndMessage();
        RuleFor(x => x.NewPassword).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا رمز عبور جدید را وارد کنید.")
            .Matches(passwordValidationInfo.Pattern).WithMessage(passwordValidationInfo.ErrorMessage)
            .MinimumLength(4).WithMessage("رمز عبور باید بیشتر از 4 کارکتر باشد.")
            .MaximumLength(40).WithMessage("رمز عبور نباید بیشتر از 40 باشد.");

        RuleFor(x => x.ReNewPassword).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا تکرار رمز عبور را وارد کنید.")
            .Equal(x => x.NewPassword).WithMessage("تکرار رمز عبور با رمز عبور وارد شده همخوانی ندارد.");
    }
}
