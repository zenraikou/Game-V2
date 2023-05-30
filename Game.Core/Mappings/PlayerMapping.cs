using Game.Contracts.Authentication;
using Game.Contracts.Generator.GenerateJWT;
using Game.Contracts.Player;
using Game.Domain.Entities;
using Mapster;

namespace Game.Core.Mappings;

public class PlayerMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PlayerRequest, Player>().Ignore("Id");
        config.NewConfig<PlayerRequest, Player>().Map(dest => dest.PasswordHash, src => src.Password);

        config.NewConfig<RegisterRequest, Player>().Ignore("Id");
        config.NewConfig<RegisterRequest, Player>().Map(dest => dest.PasswordHash, src => src.Password);

        config.ForType<Player, GenerateJWTRequest>().Map(dest => dest.Id, src => src.Id);
    }
}
