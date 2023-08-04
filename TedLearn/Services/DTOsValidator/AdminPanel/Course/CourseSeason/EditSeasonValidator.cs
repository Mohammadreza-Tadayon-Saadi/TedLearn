using FluentValidation;
using Services.DTOs.AdminPanel.Course.CourseSeason;

namespace Services.DTOsValidator.AdminPanel.Course.CourseSeason;

public class EditSeasonValidator : AbstractValidator<EditSeasonDto>
{
    public EditSeasonValidator()
    {
        RuleFor(c => c.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا عنوان فصل را وارد کنید.")
            .MaximumLength(80).WithMessage("عنوان فصل نباید بیشتر از 80 کارکتر باشد.");
    }
}
