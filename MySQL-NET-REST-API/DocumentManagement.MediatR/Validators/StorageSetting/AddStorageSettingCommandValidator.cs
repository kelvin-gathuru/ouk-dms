using DocumentManagement.MediatR.Commands;
using FluentValidation;

namespace DocumentManagement.MediatR.Validators
{
    public class AddStorageSettingCommandValidator : AbstractValidator<AddStorageSettingCommand>
    {
        public AddStorageSettingCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.JsonValue).NotEmpty().WithMessage("JsonValue is required");
        }
    }
}
