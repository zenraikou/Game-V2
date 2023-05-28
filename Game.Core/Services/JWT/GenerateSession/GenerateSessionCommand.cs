using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.JWT.GenerateSession;

public record GenerateSessionCommand(string JWT) : IRequest<SessionResponse>;
