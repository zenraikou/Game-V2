using Game.Contracts.Player;
using Game.Domain.Entities;
using Mapster;

namespace Game.Core.Mappings;

public class PlayerMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Player, PlayerResponse>()
            .Map(dest => dest.PasswordHash, src => src.PasswordHash)
            .Map(dest => dest.CreationStamp, src => src.CreationStamp);

        config.ForType<PlayerRequest, Player>()
            .BeforeMapping((src, dest) =>
            {
                if (src.Id == default && dest.Id == default)
                    dest.Id = Guid.NewGuid();

                if (src.CreationStamp == default && dest.CreationStamp == default)
                    dest.CreationStamp = DateTime.UtcNow;
            })
            .IgnoreIf((src, dest) => dest.Id != default, dest => dest.Id)
            .IgnoreIf((src, dest) => dest.CreationStamp != default, dest => dest.CreationStamp)
            .Map(dest => dest.Id, src => src.Id, src => src.Id != default)
            .Map(dest => dest.CreationStamp, src => src.CreationStamp, src => src.CreationStamp != default)
            .Map(dest => dest.PasswordHash, src => PasswordHash(src.Password))
            .Map(dest => dest.CreationStamp, src => src.CreationStamp);

        config.ForType<PlayerRequest, PlayerResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.PasswordHash, src => PasswordHash(src.Password))
            .Map(dest => dest.CreationStamp, src => src.CreationStamp);

        config.ForType<PlayerResponse, Player>()
            .Map(dest => dest.PasswordHash, src => src.PasswordHash)
            .Map(dest => dest.CreationStamp, src => src.CreationStamp);

        config.ForType<PlayerResponse, PlayerRequest>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Password, src => src.PasswordHash)
            .Map(dest => dest.CreationStamp, src => src.CreationStamp);
    }

    private static string PasswordHash(string password)
    {
        return Cypher.HashPassword(password);
    }
}
