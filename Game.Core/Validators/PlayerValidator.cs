using FluentValidation;
using Game.Domain.Entities;

namespace Game.Core.Validators;

public class PlayerValidator : AbstractValidator<Player>
{
    public PlayerValidator()
    {
        RuleFor(player => player.Name)
            .NotEmpty()
            .Length(3, 30);
    }
}
