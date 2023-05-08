using Game.Core.Common.Interfaces.Services;

namespace Game.Infrastructure.Services;

public class DateTimeProvider : IDateTImeProvider
{
    public DateTime Now => DateTime.UtcNow;
}
