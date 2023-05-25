using Game.Core.TempServices.Time;

namespace Game.Core.TempServices.TIme;

public class Time : ITime
{
    public DateTime Now => DateTime.UtcNow;
}
