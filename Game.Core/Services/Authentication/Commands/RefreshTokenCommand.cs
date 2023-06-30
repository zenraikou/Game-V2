using ErrorOr;
using Game.Contracts.Authentication;
using MediatR;

namespace Game.Core.Services.RefreshToken;

public record RefreshTokenCommand : IRequest<ErrorOr<AuthenticationResponse>>;
