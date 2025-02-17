using GamesStore.api.Dtos;
using GamesStore.api.Entities;

namespace GamesStore.Api.mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDto game)
    {
                return new Game(){  
                    Name = game.Name,
                    GenreId = game.GenreId,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate
                }; 
    }

    public static  GameDto ToDTO(this Game game)
    {
                return new(
                    game.Id,
                    game.Name,
                    game.Genre!.Name,
                    game.Price,
                    game.ReleaseDate
                );
    }
}
