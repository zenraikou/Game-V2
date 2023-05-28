using Game.Contracts.Player;
using MediatR;

namespace Game.Core.Services.JWT.GenerateJWT;

public record GenerateJWTCommand(PlayerRequest Player) : IRequest<string>;
