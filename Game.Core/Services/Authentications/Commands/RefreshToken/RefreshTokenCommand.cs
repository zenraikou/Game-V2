using ErrorOr;
using Game.Contracts.Authentication;
using MediatR;

namespace Game.Core.Services.Authentications.Commands.RefreshToken;

public record RefreshTokenCommand : IRequest<ErrorOr<AuthenticationResponse>>;
