using ErrorOr;
using Game.Core.Common.Constants;
using Game.Core.Services.Authentication.Queries;
using Game.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Game.Core.Services.Authentication.Handlers;

public class GetFingerprintHandler : IRequestHandler<GetFingerprintQuery, ErrorOr<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetFingerprintHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<string>> Handle(GetFingerprintQuery request, CancellationToken cancellationToken)
    {
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers[HTTPHeaders.Fingerprint].ToString();

        if (string.IsNullOrEmpty(fingerprint))
        {
            return Errors.Authorization.Unauthorized;
        }

        return await Task.FromResult(fingerprint);
    }
}
