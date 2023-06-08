using Game.Core.Common.Constants;
using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Game.Core.Services.Authentication.Handlers;

public class GetFingerprintHandler : IRequestHandler<GetFingerprintQuery, string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetFingerprintHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> Handle(GetFingerprintQuery request, CancellationToken cancellationToken)
    {
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers[HTTPHeaders.Fingerprint].ToString();

        if (string.IsNullOrEmpty(fingerprint))
        {
            throw new UnauthorizedException("Access denied.");
        }

        return await Task.FromResult(fingerprint);
    }
}
