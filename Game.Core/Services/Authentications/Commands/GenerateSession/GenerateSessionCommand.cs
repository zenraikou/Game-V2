using ErrorOr;
using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Authentications.Commands.GenerateSession;

public record GenerateSessionCommand(string JWT) : IRequest<ErrorOr<SessionResponse>>;
