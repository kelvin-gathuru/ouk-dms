using DocumentManagement.MediatR.Commands;
using FluentValidation;

namespace DocumentManagement.MediatR.Validators
{
    public class AddDocumentStatusCommandValidator: AbstractValidator<AddDocumentStatusCommand>
    {
        public AddDocumentStatusCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.Description).NotEmpty().WithMessage("Description is required");
        }
    }
}
