namespace Game.Core.Common.Interfaces.Authentication;

public interface IFingerprintService
{
    Task ValidateFingerprint();
}
