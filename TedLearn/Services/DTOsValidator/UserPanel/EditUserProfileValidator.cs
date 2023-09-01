using Core.Securities;
using FluentValidation;
using Services.DTOs.UserPanel.Home;

namespace Services.DTOsValidator.UserPanel;

public class EditUserProfileValidator : AbstractValidator<EditUserProfileDto>
{
    public EditUserProfileValidator()
    {
        RuleFor(u => u.FirstName)
            .MaximumLength(70).WithMessage("نام نباید بیشتر از 70 کارکتر باشد.");

        RuleFor(u => u.LastName)
            .MaximumLength(70).WithMessage("نام خانوادگی نباید بیشتر از 70 کارکتر باشد.");

        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
            .Matches(RegularExpression.EmailRegularExpression()).WithMessage("ایمیل وارد شده صحیح نیست.")
            .MaximumLength(100).WithMessage("نام خانوادگی نباید بیشتر از 70 کارکتر باشد.");

        RuleFor(u => u.Biography)
            .MaximumLength(1000).WithMessage("بیوگرافی نباید بیشتر از 1000 کارکتر باشد.");

        RuleFor(u => u.WebsiteAddress).Cascade(CascadeMode.Stop)
            .Matches(RegularExpression.UrlRegularExpression()).WithMessage("آدرس سایت وارد شده صحیح نیست.")
            .MaximumLength(100).WithMessage("آدرس سایت نباید بیشتر از 100 کارکتر باشد.");
    }
}
