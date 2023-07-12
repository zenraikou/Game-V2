using Game.Contracts.Session;
using Game.Domain.Entities;
using Mapster;

namespace Game.Core.Mappers;

public class SessionMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Session, SessionRequest>()
            .Map(dest => dest.Id, src => src.Id);

        config.ForType<SessionRequest, Session>()
            .BeforeMapping((src, dest) =>
            {
                if (src.Id == default && dest.Id == default)
                    dest.Id = Guid.NewGuid();
            })
            .IgnoreIf((src, dest) => dest.Id != default, dest => dest.Id)
            .Map(dest => dest.Id, src => src.Id, src => src.Id != default);

        config.ForType<SessionRequest, SessionResponse>()
            .Map(dest => dest.Id, src => src.Id);

        config.ForType<SessionResponse, SessionRequest>()
            .Map(dest => dest.Id, src => src.Id);
    }
}
