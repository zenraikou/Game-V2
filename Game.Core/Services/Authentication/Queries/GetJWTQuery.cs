using MediatR;

namespace Game.Core.Services.Authentication.Queries;
public record GetJWTQuery : IRequest<string>;
