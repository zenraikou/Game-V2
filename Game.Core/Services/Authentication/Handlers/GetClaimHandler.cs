using ErrorOr;
using Game.Core.Services.Authentication.Queries;
using Game.Domain.Common.Errors;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace Game.Core.Services.Authentication.Handlers;

public class GetClaimHandler : IRequestHandler<GetClaimQuery, ErrorOr<string>>
{
    private readonly ISender _mediator;

    public GetClaimHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<ErrorOr<string>> Handle(GetClaimQuery request, CancellationToken cancellationToken)
    {
        var claim = string.Empty;
        var handler = new JwtSecurityTokenHandler();

        if (string.IsNullOrEmpty(request.JWT))
        {
            var jwt = await _mediator.Send(new GetJWTQuery());

            claim = handler.ReadJwtToken(jwt).Claims.FirstOrDefault(request.Expression)?.Value;
        }
        else
        {
            claim = handler.ReadJwtToken(request.JWT).Claims.FirstOrDefault(request.Expression)?.Value;
        }

        if (string.IsNullOrEmpty(claim))
        {
            return Errors.Authorization.Unauthorized;
        }

        return claim;
    }
}
