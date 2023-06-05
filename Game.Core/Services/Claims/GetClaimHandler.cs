using System.IdentityModel.Tokens.Jwt;
using Game.Core.Exceptions;
using Game.Core.Services.Header;
using MediatR;

namespace Game.Core.Services.Claims;

public class GetClaimHandler : IRequestHandler<GetClaimQuery, string>
{
    private readonly ISender _mediator;

    public GetClaimHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<string> Handle(GetClaimQuery request, CancellationToken cancellationToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var claim = string.Empty;

        if (string.IsNullOrEmpty(request.JWT))
        {
            var getHeaderQuery = new GetHeaderQuery("Authorization");
            var header = await _mediator.Send(getHeaderQuery);

            var jwt = header?.Split(' ')[1];

            claim = handler.ReadJwtToken(jwt).Claims.FirstOrDefault(request.Expression)?.Value;
        }
        else
        {
            claim = handler.ReadJwtToken(request.JWT).Claims.FirstOrDefault(request.Expression)?.Value;
        }

        if (string.IsNullOrEmpty(claim))
        {
            throw new UnauthorizedException("Access denied.");
        }

        return await Task.FromResult(claim);
    }
}
