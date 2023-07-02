using FluentValidation;

namespace Game.Core.Services.Authentications.Commands.Register;

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
