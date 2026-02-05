using DocumentManagement.MediatR.Commands;
using FluentValidation;

namespace DocumentManagement.MediatR.Validators
{
    public class AddScreenCommandValidator:  AbstractValidator<AddScreenCommand>
    {
        public AddScreenCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}
