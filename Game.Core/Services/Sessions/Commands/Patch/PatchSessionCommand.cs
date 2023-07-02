using ErrorOr;
using Game.Contracts.Session;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Game.Core.Services.Sessions.Commands.Patch;

public record PatchSessionCommand(Guid Id, JsonPatchDocument<SessionRequest> JsonPatchDocument) : IRequest<ErrorOr<Updated>>;
