using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Authentication.Commands;

public record GenerateSessionCommand(string JWT) : IRequest<SessionResponse>;
