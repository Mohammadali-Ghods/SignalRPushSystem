using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWithWebAPIAndMongoDB.Validations
{
    public class FriendValidation : AbstractValidator<Entities.Friend>
    {
        public FriendValidation()
        {
            //RuleFor(p => p.FullName)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull()
            //    .MinimumLength(15).WithMessage("{PropertyName} must at least 15 characters.")
            //    .MaximumLength(150).WithMessage("{PropertyName} must not exceed 150 characters.");
        }

    }
}
