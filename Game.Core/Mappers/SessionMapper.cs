using Game.Contracts.Session;
using Game.Domain.Entities;
using Mapster;

namespace Game.Core.Mappers;

public class SessionMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<SessionRequest, Session>()
            .BeforeMapping((src, dest) =>
            {
                if (src.Id == Guid.Empty && dest.Id == Guid.Empty)
                    dest.Id = Guid.NewGuid();
            })
            .IgnoreIf((src, dest) => dest.Id != Guid.Empty, dest => dest.Id)
            .Map(dest => dest.Id, src => src.Id, src => src.Id != Guid.Empty);

        config.ForType<SessionResponse, SessionRequest>()
            .Map(dest => dest.Id, src => src.Id, src => src.Id != Guid.Empty);
    }
}
