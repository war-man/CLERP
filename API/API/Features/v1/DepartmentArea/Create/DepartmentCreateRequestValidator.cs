﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLERP.API.Features.v1.DepartmentArea.Create
{
    public class DepartmentCreateRequestValidator : AbstractValidator<DepartmentCreateRequest>
    {
        private const int minTitleLenght = 2;
        private const int maxTitleLenght = 30;

        public DepartmentCreateRequestValidator()
        {
            RuleFor(d => d.Title)
                .NotNull()
                .NotEmpty()
                .MinimumLength(minTitleLenght)
                .MaximumLength(maxTitleLenght);
        }
    }
}