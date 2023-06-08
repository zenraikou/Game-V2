using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Queries;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace Game.Core.Services.Authentication.Handlers;

public class GetClaimHandler : IRequestHandler<GetClaimQuery, string>
{
    private readonly ISender _mediator;

    public GetClaimHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<string> Handle(GetClaimQuery request, CancellationToken cancellationToken)
    {
        var claim = string.Empty;
        var handler = new JwtSecurityTokenHandler();

        if (string.IsNullOrEmpty(request.JWT))
        {
            var getJWTQuery = new GetJWTQuery();
            var jwt = await _mediator.Send(getJWTQuery);

            claim = handler.ReadJwtToken(jwt).Claims.FirstOrDefault(request.Expression)!.Value;
        }
        else
        {
            claim = handler.ReadJwtToken(request.JWT).Claims.FirstOrDefault(request.Expression)!.Value;
        }

        if (string.IsNullOrEmpty(claim))
        {
            throw new UnauthorizedException("Access denied.");
        }

        return claim;
    }
}
