using FluentValidation;
using Services.DTOs.AdminPanel.Course.CourseGroup;

namespace Services.DTOsValidator.AdminPanel.Course;

public class AddSubGroupValidator : AbstractValidator<AddSubGroupDto>
{
    public AddSubGroupValidator()
    {
        RuleFor(cg => cg.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا عنوان زیر گروه را وارد کنید.")
            .MaximumLength(70).WithMessage("عنوان زیر گروه نباید بیشتر از 70 کارکتر باشد.");

        RuleFor(cg => cg.ParentId).NotNull().WithMessage("لطفا گروه را انتخاب کنید.");
    }
}
