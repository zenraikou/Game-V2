using Game.Contracts.Generator.GenerateSession;
using MediatR;

namespace Game.Core.Services.Generator.GenerateSession;

public record GenerateSessionCommand(GenerateSessionRequest GenerateSession) : IRequest<GenerateSessionResponse>;
