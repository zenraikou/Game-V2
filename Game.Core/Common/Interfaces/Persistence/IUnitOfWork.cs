namespace Game.Core.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    ISessionRepository Sessions { get; }
    IPlayerRepository Players { get; }
    Task Save();
}
