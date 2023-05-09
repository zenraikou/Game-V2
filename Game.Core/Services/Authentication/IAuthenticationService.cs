namespace Game.Core.Services.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult Login(string uniqueName, string password);
    AuthenticationResult Register(string handle, string name, string uniqueName, string email, string password);
}
