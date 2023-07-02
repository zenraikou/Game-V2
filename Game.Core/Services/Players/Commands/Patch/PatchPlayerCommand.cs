using ErrorOr;
using Game.Contracts.Player;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Game.Core.Services.Players.Commands.Patch;

public record PatchPlayerCommand(Guid Id, JsonPatchDocument<PlayerRequest> JsonPatchDocument) : IRequest<ErrorOr<Updated>>;
