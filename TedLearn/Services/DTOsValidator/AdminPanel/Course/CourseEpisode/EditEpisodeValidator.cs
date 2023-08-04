using Core.Utilities;
using FluentValidation;
using Services.Dto.AdminPanel.Course.CourseEpisode;

namespace Services.DTOsValidator.AdminPanel.Course.CourseEpisode;

public class EditEpisodeValidator : AbstractValidator<EditEpisodeDto>
{
    public EditEpisodeValidator()
    {
        RuleFor(c => c.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا عنوان قسمت را وارد کنید.")
            .MaximumLength(80).WithMessage("عنوان قسمت نباید بیشتر از 80 کارکتر باشد.");

        When(c => c.File != null, () =>
        {
            RuleFor(c => c.File).Cascade(CascadeMode.Stop)
            .Must(c => FileChecker.IsRarFile(c)).WithMessage("لطفا فقط فایل rar بارگذاری کنید.");
        });

        RuleFor(c => c.Time).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا زمان قسمت را وارد کنید.")
            .Must(timeSpan => timeSpan >= TimeSpan.Zero).WithMessage("لطفا زمان قسمت را به درستی وارد کنید.");
    }
}
