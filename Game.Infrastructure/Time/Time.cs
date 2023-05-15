using Game.Core.Common.Interfaces.Time;

namespace Game.Infrastructure.Services;

public class Time : ITime
{
    public DateTime Now => DateTime.UtcNow;
}
