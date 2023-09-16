using Backend.Domain.Warriors;
using FluentValidation;

namespace Backend.Application.Warriors.CreateWarrior;

internal sealed class CreateWarriorCommandValidator : AbstractValidator<CreateWarriorCommand>
{
    public CreateWarriorCommandValidator()
    {
        RuleFor(w => w.WarriorName)
            .NotNull()
            .NotEmpty()
            .MinimumLength(WarriorName.MinLength)
            .WithErrorCode(WarriorErrors.InvalidName.Code)
            .WithMessage(WarriorErrors.InvalidName.Message);
    }
}
