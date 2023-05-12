namespace Game.Core.Common.Interfaces.Authentication;

public interface IUserService
{
    List<string>? GetCurrentUserUniqueName();
}
