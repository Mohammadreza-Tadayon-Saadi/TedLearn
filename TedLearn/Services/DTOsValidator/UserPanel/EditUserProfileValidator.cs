using Core.Securities;
using FluentValidation;
using Services.DTOs.UserPanel;

namespace Services.DTOsValidator.UserPanel;

public class EditUserProfileValidator : AbstractValidator<EditUserProfileDto>
{
    public EditUserProfileValidator()
    {
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
