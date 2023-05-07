namespace Game.Core.Services.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult Login(string email, string password);
    AuthenticationResult Register(string name, string handle, string email, string password);
}
