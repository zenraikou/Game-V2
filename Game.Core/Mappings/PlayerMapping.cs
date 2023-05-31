using Game.Contracts.Player;
using Game.Domain.Entities;
using Mapster;

namespace Game.Core.Mappings;

public class PlayerMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForDestinationType<Player>()
            .Ignore(dest => dest.Id);

        config.NewConfig<PlayerRequest, Player>()
            .Map(dest => dest.PasswordHash, src => src.Password);
    }
}
