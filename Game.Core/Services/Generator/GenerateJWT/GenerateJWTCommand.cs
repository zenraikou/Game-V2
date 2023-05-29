using Game.Contracts.Generator.GenerateJWT;
using MediatR;

namespace Game.Core.Services.Generator.GenerateJWT;

public record GenerateJWTCommand(GenerateJWTRequest JWT) : IRequest<GenerateJWTResponse>;
