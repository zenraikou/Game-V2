using MediatR;
using Microsoft.AspNetCore.Http;

namespace Game.Core.Services.Header;

public class GetHeaderHandler : IRequestHandler<GetHeaderQuery, string?>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string?> Handle(GetHeaderQuery request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        var response = context?.Request.Headers[request.Header].ToString();
        return await Task.FromResult(response);
    }
}
