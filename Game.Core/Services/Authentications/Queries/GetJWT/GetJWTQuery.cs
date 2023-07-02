using ErrorOr;
using MediatR;

namespace Game.Core.Services.Authentications.Queries.GetJWT;

public record GetJWTQuery : IRequest<ErrorOr<string>>;
