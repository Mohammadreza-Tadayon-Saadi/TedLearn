﻿using FluentValidation;
using Services.DTOs.AdminPanel.Role;

namespace Services.DTOsValidator.AdminPanel.Role;

public class AddRoleValidator : AbstractValidator<AddRoleDto>
{
    public AddRoleValidator()
    {
        RuleFor(u => u.RoleName).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("لطفا نام نقش را وارد کنید.")
            .MaximumLength(70).WithMessage("نام نقش نباید بیشتر از 70 کارکتر باشد.");
    }
}
