using Game.Domain.Entities;
using MediatR;

namespace Game.Core.Services.Generator.GenerateSession;

public record GenerateSessionCommand(string JWT) : IRequest<Session>;
