using Game.API;
using Game.Contracts;
using Game.Core;
using Game.Domain;
using Game.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAPI();
    builder.Services.AddContracts();
    builder.Services.AddCore();
    builder.Services.AddDomain();
    builder.Services.AddInfrastructure();
}

var app = builder.Build();
{
    app.UseAPI();
    app.Run();
}
