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
            group.MapGet("/",async (GameStoreContext dbContext) 
                => await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDTO())
                .AsNoTracking()
                .ToListAsync());

            // GET /Games/1
            group.MapGet("/{id}/",async (int id, GameStoreContext dbContext) => 
            {
                Game? game = await dbContext.Games.FindAsync(id);

                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDTO);
            })
            .WithName(GetGameEndpointName);

            // POST /Games
            group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {       
                Game game = newGame.ToEntity();      

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();
               
                return Results.CreatedAtRoute
                (GetGameEndpointName, new { id = game.Id },
                game.ToGameDetailsDTO());

            });

            // PUT /Games
            group.MapPut("/{id}",async (int id, UpdateGameDto updateGame, GameStoreContext
             dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if(existingGame is null)
                {
                    return Results.NotFound();
                }

                dbContext.Entry(existingGame)
                .CurrentValues
                .SetValues(updateGame.ToEntity(id));

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });


            // DELETE/Games/1
            group.MapDelete("/{id}" ,async (int id, GameStoreContext dbContext) => 
            {
                await dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDeleteAsync();            

                return Results.NotFound();
            });

            //app.MapGet("/", () => "Hello World!");

            return group;

        }
}
