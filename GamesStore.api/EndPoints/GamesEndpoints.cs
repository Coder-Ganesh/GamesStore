using System;
using GamesStore.api.Dtos;
using GamesStore.api.Entities;
using GamesStore.Api.mapping;
using GameStore.api.Data;
using Microsoft.EntityFrameworkCore;

namespace GamesStore.api.EndPoints;

public static class GamesEndpoints
{
   const string GetGameEndpointName = "GetGame";

        public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
        {
            var group = app.MapGroup("Games").WithParameterValidation();

             // GET /Games
            group.MapGet("/", (GameStoreContext dbContext) 
                => dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDTO())
                .AsNoTracking());

            // GET /Games/1
            group.MapGet("/{id}/", (int id, GameStoreContext dbContext) => 
            {
                Game? game = dbContext.Games.Find(id);

                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDTO);
            })
            .WithName(GetGameEndpointName);

            // POST /Games
            group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
            {       
                Game game = newGame.ToEntity();      

                dbContext.Games.Add(game);
                dbContext.SaveChanges();
               
                return Results.CreatedAtRoute
                (GetGameEndpointName, new { id = game.Id },
                game.ToGameDetailsDTO());

            });

            // PUT /Games
            group.MapPut("/{id}", (int id, UpdateGameDto updateGame, GameStoreContext
             dbContext) =>
            {
                var existingGame = dbContext.Games.Find(id);

                if(existingGame is null)
                {
                    return Results.NotFound();
                }

                dbContext.Entry(existingGame)
                .CurrentValues
                .SetValues(updateGame.ToEntity(id));

                dbContext.SaveChanges();

                return Results.NoContent();
            });


            // DELETE/Games/1
            group.MapDelete("/{id}" , (int id, GameStoreContext dbContext) => 
            {
                dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDelete();            

                return Results.NotFound();
            });

            //app.MapGet("/", () => "Hello World!");

            return group;

        }
}
