using MediatR;

namespace Game.Core.Services.Header;

public record GetHeaderQuery(string Header) : IRequest<string?>;
