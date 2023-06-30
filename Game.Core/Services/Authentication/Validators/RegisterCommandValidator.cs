using FluentValidation;
using Game.Core.Services.Authentication.Commands;

namespace Game.Core.Services.Authentication.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Register.Handle).NotEmpty();
        RuleFor(x => x.Register.Name).NotEmpty();
        RuleFor(x => x.Register.UniqueName).NotEmpty();
        RuleFor(x => x.Register.Email).NotEmpty();
        RuleFor(x => x.Register.Password).NotEmpty();
        RuleFor(x => x.Register.Role).NotEmpty();
    }
}
