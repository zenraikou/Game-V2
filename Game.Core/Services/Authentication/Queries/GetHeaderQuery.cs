using MediatR;

namespace Game.Core.Services.Authentication.Queries;

public record GetHeaderQuery(string Header) : IRequest<string?>;
