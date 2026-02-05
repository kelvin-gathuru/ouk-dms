using DocumentManagement.MediatR.Commands;
using FluentValidation;

namespace DocumentManagement.MediatR.Validators
{
    public class UpdateDocumentStatusCommandValidator : AbstractValidator<UpdateDocumentStatusCommand>
    {
        public UpdateDocumentStatusCommandValidator() 
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.Description).NotEmpty().WithMessage("Description is required");
        }
    }
}
