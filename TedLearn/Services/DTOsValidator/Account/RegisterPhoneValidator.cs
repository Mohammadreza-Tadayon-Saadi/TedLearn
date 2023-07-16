using FluentValidation;
using Services.DTOs.Account;

namespace Services.DTOsValidator.Account;

public class RegisterPhoneValidator : AbstractValidator<RegisterPhoneDto>
{
    public RegisterPhoneValidator()
    {
        RuleFor(r => r.Code).NotEmpty().WithMessage("لطفا کد فعالسازی را وارد کنید.");
    }
}
