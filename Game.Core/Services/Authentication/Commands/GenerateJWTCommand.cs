using MediatR;

namespace Game.Core.Services.Authentication.Commands;

public record GenerateJWTCommand(string Id, string Role, string? JTI = null) : IRequest<string>;
