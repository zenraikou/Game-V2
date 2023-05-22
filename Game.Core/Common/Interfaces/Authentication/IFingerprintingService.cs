namespace Game.Core.Common.Interfaces.Authentication;

public interface IFingerprintingService
{
    Task Validate();
}
