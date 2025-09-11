using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = "Data Source=GameStore.db";
builder.Services.AddSqlite<GameStore.Api.Data.GameStoreContext>(connString);
var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
