using FluentValidation;
using Services.DTOs.AdminPanel.Course.CourseGroup;

namespace Services.DTOsValidator.AdminPanel.Course;

public class AddGroupValidator : AbstractValidator<AddGroupDto>
{
    public AddGroupValidator()
    {
        RuleFor(cg => cg.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا عنوان گروه را وارد کنید.")
            .MaximumLength(70).WithMessage("عنوان گروه نباید بیشتر از 70 کارکتر باشد.");
    }
}
