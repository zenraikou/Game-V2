using Game.Domain.Entities;
using MediatR;

namespace Game.Core.Services.Generator.GenerateJWT;

public record GenerateJWTCommand(Player Player) : IRequest<string>;
