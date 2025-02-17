using System;
using GamesStore.api.Dtos;
using GamesStore.api.Entities;
using GamesStore.Api.mapping;
using GameStore.api.Data;

namespace GamesStore.api.EndPoints;

public static class GamesEndpoints
{
   const string GetGameEndpointName = "GetGame";

        private static readonly List<GameDto> games = [
            new(1,"Street Fighter II","Fighting",19.99M,new DateOnly(1992,7,15)),
            new(2,"Final Fantasy XIV","Role Playing",59.99M,new DateOnly(2010,9,30)),
            new(3,"FIFA 23","Sports",69.99M,new DateOnly(2022,9,27))
        ];


        public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
        {
            var group = app.MapGroup("Games").WithParameterValidation();

             // GET /Games
            group.MapGet("/", () => games);

            // GET /Games/1
            group.MapGet("/{id}/", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndpointName);

            // POST /Games
            group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
            {       
                Game game = newGame.ToEntity();
                game.Genre = dbContext.Genre.Find(newGame.GenreId);       

                dbContext.Games.Add(game);
                dbContext.SaveChanges();
               
                return Results.CreatedAtRoute
                (GetGameEndpointName, new { id = game.Id },
                game.ToDTO());
                
            });

            // PUT /Games
            group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
            {
                var index = games.FindIndex(game => game.Id == id);

                games[index] = new GameDto(
                    id,
                    updateGame.Name,
                    updateGame.Genre,
                    updateGame.Price,
                    updateGame.ReleaseDate
                );

                return Results.NoContent;
            });


            // DELETE/Games/1
            group.MapDelete("/{id}" , (int id) => 
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NotFound();
            });

            //app.MapGet("/", () => "Hello World!");

            return group;

        }
}
