﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLERP.API.Features.v1.EmployeeArea.Login
{
    public class EmployeeLoginDtoValidator : AbstractValidator<EmployeeLoginDto>
    {
        public EmployeeLoginDtoValidator()
        {
            RuleFor(eld => eld.Username)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(eld => eld.Password)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
