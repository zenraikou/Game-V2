namespace Game.Core.Services.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult Login(string email, string password);
    AuthenticationResult Register(string handle, string name, string uniqueName, string email, string password);
}
