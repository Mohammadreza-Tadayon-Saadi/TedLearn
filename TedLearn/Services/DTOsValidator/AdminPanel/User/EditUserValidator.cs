using Core.Securities;
using Core.Utilities;
using FluentValidation;
using Services.DTOs.AdminPanel.User;

namespace Services.DTOsValidator.AdminPanel.User;

public class EditUserValidator : AbstractValidator<EditUserDto>
{
    public EditUserValidator()
    {
        RuleFor(x => x.PhoneNumber).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا شماره تلفن را وارد کنید.")
            .Matches(RegularExpression.PhoneRegularExpression()).WithMessage("شماره تلفن وارد شده صحیح نیست.")
            .Length(11).WithMessage("شماره تلفن وارد شده صحیح نیست.");

        var passwordValidationInfo = RegularExpression.GetPasswordValidationPatternAndMessage();
        When(x => x.Password.HasValue(), () =>
        {
            RuleFor(x => x.Password).Cascade(CascadeMode.Stop)
                .Matches(passwordValidationInfo.Pattern).WithMessage(passwordValidationInfo.ErrorMessage)
                .MinimumLength(4).WithMessage("رمز عبور باید بیشتر از 4 کارکتر باشد.")
                .MaximumLength(40).WithMessage("رمز عبور نباید بیشتر از 44 کارکتر باشد.");
        });

        When(x => x.Amount.HasValue, () =>
        {
            RuleFor(t => t.Amount).Cascade(CascadeMode.Stop)
                .InclusiveBetween(1000, 2500000).WithMessage("مبلغ فقط میتواند بین 1000 و 2500000 باشد.");
        });

        RuleFor(x => x.FirstName)
            .MaximumLength(70).WithMessage("نام نباید بیشتر از 70 کارکتر باشد.");

        RuleFor(x => x.LastName)
            .MaximumLength(70).WithMessage("نام خانوادگی نباید بیشتر از 70 کارکتر باشد.");

        RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
            .Matches(RegularExpression.EmailRegularExpression()).WithMessage("ایمیل وارد شده صحیح نیست.")
            .MaximumLength(100).WithMessage("نام خانوادگی نباید بیشتر از 70 کارکتر باشد.");

        RuleFor(x => x.Biography)
            .MaximumLength(1000).WithMessage("بیوگرافی نباید بیشتر از 1000 کارکتر باشد.");

        RuleFor(x => x.WebsiteAddress).Cascade(CascadeMode.Stop)
            .Matches(RegularExpression.UrlRegularExpression()).WithMessage("آدرس سایت وارد شده صحیح نیست.")
            .MaximumLength(100).WithMessage("آدرس سایت نباید بیشتر از 100 کارکتر باشد.");
    }
}
