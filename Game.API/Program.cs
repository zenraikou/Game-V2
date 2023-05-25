using Game.API;
using Game.Core;
using Game.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAPI(builder.Configuration);
    builder.Services.AddCore(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    app.UseAPI();
    app.Run();
}
