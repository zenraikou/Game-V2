using Game.Contracts.Player;
using Game.Domain.Entities;
using Mapster;

namespace Game.Core.Mappings;

public class PlayerMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForDestinationType<Player>()
            .Ignore(dest => dest.Id);

        config.ForType<Player, PlayerResponse>()
            .Map(dest => dest.PasswordHash, src => src.PasswordHash);

        config.ForType<PlayerRequest, Player>()
            .Map(dest => dest.PasswordHash, src => PasswordHash(src.Password));

        config.ForType<PlayerRequest, PlayerResponse>()
            .Map(dest => dest.PasswordHash, src => PasswordHash(src.Password));

        config.ForType<PlayerResponse, PlayerRequest>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Password, src => src.PasswordHash);
    }

    private static string PasswordHash(string password)
    {
        return Cypher.HashPassword(password);
    }
}
