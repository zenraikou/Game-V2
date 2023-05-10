using Game.API;
using Game.Contracts;
using Game.Core;
using Game.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAPI(builder.Configuration);
    builder.Services.AddContracts();
    builder.Services.AddCore();
    builder.Services.AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    app.UseAPI();
    app.Run();
}
