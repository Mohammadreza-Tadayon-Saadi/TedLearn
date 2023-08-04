using Core.Utilities;
using FluentValidation;
using Services.DTOs.AdminPanel.Course;

namespace Services.DTOsValidator.AdminPanel.Course;

public class EditCourseValidator : AbstractValidator<EditCourseDto>
{
    public EditCourseValidator()
    {
        RuleFor(c => c.CourseTitle).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا عنوان دوره را وارد کنید.")
            .MaximumLength(80).WithMessage("عنوان دوره نباید بیشتر از 80 کارکتر باشد.");

        RuleFor(c => c.GroupId)
            .NotEmpty().WithMessage("لطفا گروه دوره را انتخاب کنید.");

        RuleFor(c => c.TeacherId)
            .NotEmpty().WithMessage("لطفا استاد دوره را انتخاب کنید.");

        RuleFor(c => c.StatusId)
            .NotEmpty().WithMessage("لطفا وضعیت دوره را انتخاب کنید.");

        RuleFor(c => c.CourseLevel)
            .NotEmpty().WithMessage("لطفا سطح دوره را انتخاب کنید.");

        When(c => c.DemoFile != null, () =>
        {
            RuleFor(c => c.DemoFile).Must(c => FileChecker.IsVideo(c)).WithMessage("لطفا فقط ویدیو بارگذاری کنید.");
        });

        When(c => c.ImageFile != null, () =>
        {
            RuleFor(c => c.ImageFile).Must(c => FileChecker.IsImage(c)).WithMessage("لطفا فقط عکس بارگذاری کنید.");
        });

        RuleFor(c => c.Requirment)
            .MaximumLength(500).WithMessage("پیش نیاز های دوره نباید بیشتر از 500 کارکتر باشد.");

        RuleFor(c => c.Tags)
            .MaximumLength(500).WithMessage("برچسب های دوره نباید بیشتر از 500 کارکتر باشد.");

        RuleFor(c => c.Description).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا توضیحات دوره را وارد کنید.")
            .MaximumLength(4000).WithMessage("توضیحات دوره نباید بیشتر از 4000 کارکتر باشد.");
    }
}
