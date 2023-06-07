using Game.Contracts.Generator.GenerateJWT;
using MediatR;

namespace Game.Core.Services.Authentication.Commands;

public record GenerateJWTCommand(GenerateJWTRequest GenerateJWT) : IRequest<GenerateJWTResponse>;
