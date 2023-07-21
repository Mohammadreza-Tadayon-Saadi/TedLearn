using FluentValidation;
using Services.DTOs.AdminPanel.Course.CourseGroup;

namespace Services.DTOsValidator.AdminPanel.Course.CourseGroup;

public class EditSubGroupValidator : AbstractValidator<EditSubGroupDto>
{
    public EditSubGroupValidator()
    {
        RuleFor(cg => cg.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا عنوان گروه را وارد کنید.")
            .MaximumLength(70).WithMessage("عنوان گروه نباید بیشتر از 70 کارکتر باشد.");
    }
}
