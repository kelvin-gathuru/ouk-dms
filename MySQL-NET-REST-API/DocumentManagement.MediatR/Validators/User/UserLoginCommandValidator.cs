using DocumentManagement.MediatR.Commands;
using FluentValidation;

namespace DocumentManagement.MediatR.Validators
{
    public class UserLoginCommandValidator: AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
        {
            RuleFor(c => c.UserName).NotEmpty().WithMessage("Please enter username.");
            RuleFor(c => c.Password).NotEmpty().WithMessage("Please enter password.");
        }
    }
}
