using Game.Contracts.Generator.GenerateSession;
using MediatR;

namespace Game.Core.Services.Authentication.Commands;

public record GenerateSessionCommand(GenerateSessionRequest GenerateSession) : IRequest<GenerateSessionResponse>;
