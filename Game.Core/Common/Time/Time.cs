using Game.Core.Common.Interfaces.Time;

namespace Game.Core.Common.Time;

public class Time : ITime
{
    public DateTime Now => DateTime.UtcNow;
}
