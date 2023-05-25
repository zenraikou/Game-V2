namespace Game.Core.Common.Interfaces.Persistence;

public interface IUnitOfWork : IAsyncDisposable
{
    ISessionRepository Sessions { get; }
    IPlayerRepository Players { get; }
    Task Save();
}
