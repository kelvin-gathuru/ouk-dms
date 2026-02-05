using DocumentManagement.MediatR.Commands;
using FluentValidation;

namespace DocumentManagement.MediatR.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.UserName).NotEmpty().WithMessage("Please enter username.");
            RuleFor(c => c.FirstName).NotEmpty().WithMessage("Please enter firstname.");
            RuleFor(c => c.LastName).NotEmpty().WithMessage("Please enter lastname.");
            RuleFor(c => c.Email).NotEmpty().WithMessage("Please enter email .");
            RuleFor(c => c.Email).EmailAddress().WithMessage("Email in right format.");
            RuleFor(c => c.Password).NotEmpty().WithMessage("Please enter password.");
        }
    }
}
