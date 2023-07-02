using MediatR;

namespace Game.Core.Services.Authentications.Commands.GenerateJWT;

public record GenerateJWTCommand(string Id, string Role, string? JTI = null) : IRequest<string>;
