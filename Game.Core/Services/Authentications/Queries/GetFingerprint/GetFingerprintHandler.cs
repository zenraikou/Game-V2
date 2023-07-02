using ErrorOr;
using Game.Core.Common.Constants;
using Game.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Game.Core.Services.Authentications.Queries.GetFingerprint;

public class GetFingerprintHandler : IRequestHandler<GetFingerprintQuery, ErrorOr<string>>
{
    private readonly IHttpContextAccessor _accessor;

    public GetFingerprintHandler(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public async Task<ErrorOr<string>> Handle(GetFingerprintQuery request, CancellationToken cancellationToken)
    {
        var fingerprint = _accessor.HttpContext?.Request.Headers[HTTPHeaders.Fingerprint].ToString();

        if (string.IsNullOrEmpty(fingerprint))
        {
            return Errors.Authorization.Unauthorized;
        }

        return await Task.FromResult(fingerprint);
    }
}
