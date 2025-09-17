using System;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameSummaryDto> games = [ new GameSummaryDto(1, "The Legend of Zelda: Breath of the Wild", "Action-adventure", 59.99m, new DateOnly(2017, 3, 3)),
    new GameSummaryDto(2, "God of War", "Action-adventure", 49.99m, new DateOnly(2018, 4, 20)),
    new GameSummaryDto(3, "Red Dead Redemption 2", "Action-adventure", 39.99m, new DateOnly(2018, 10, 26)),
    new GameSummaryDto(4, "The Witcher 3: Wild Hunt", "Action RPG", 29.99m, new DateOnly(2015, 5, 19)),
    new GameSummaryDto(5, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18))];


    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();

        //GET /games
        group.MapGet("/", () => games);

        //GET /games/{id}
        group.MapGet("/{id}", (int id,GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);
            return game is null ? Results.NotFound() : Results.Ok(game);

        }).WithName(GetGameEndpointName);

        //POST /games
        group.MapPost("/", (CreateGameDto newGame,GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                Genre = dbContext.Genres.Find(newGame.GenreId)!,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };


            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        //PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameSummaryDto(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );

            return Results.NoContent();
        });

        //DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });

        return group;

    }

}
