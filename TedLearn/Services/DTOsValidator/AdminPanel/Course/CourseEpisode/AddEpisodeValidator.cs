using Core.Utilities;
using FluentValidation;
using Services.DTOs.AdminPanel.Course.CourseEpisode;

namespace Services.DTOsValidator.AdminPanel.Course.CourseEpisode;

public class AddEpisodeValidator : AbstractValidator<AddEpisodeDto>
{
    public AddEpisodeValidator()
    {
        RuleFor(c => c.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا عنوان قسمت را وارد کنید.")
            .MaximumLength(80).WithMessage("عنوان قسمت نباید بیشتر از 80 کارکتر باشد.");

        RuleFor(c => c.EpisodeFile).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا فایل مربوط به قسمت را وارد کنید.")
            .Must(c => FileChecker.IsRarFile(c)).WithMessage("لطفا فقط فایل rar بارگذاری کنید.");

        RuleFor(c => c.Time).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("لطفا زمان قسمت را وارد کنید.")
            .Must(timeSpan => timeSpan >= TimeSpan.Zero).WithMessage("لطفا زمان قسمت را به درستی وارد کنید.");
    }
}
