using Game.Contracts.Session;
using Game.Domain.Entities;
using Mapster;

namespace Game.Core.Mappers;

public class SessionMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // config.ForType<SessionRequest, Session>()
        //     .Map(dest => dest.Id, src => src.Id);

        // config.ForType<SessionResponse, SessionRequest>()
        //     .Map(dest => dest.Id, src => src.Id);
    }
}
