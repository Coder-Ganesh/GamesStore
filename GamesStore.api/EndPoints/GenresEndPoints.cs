using GamesStore.api.Mapping;
using GameStore.api.Data;
using Microsoft.EntityFrameworkCore;

namespace GamesStore.api.EndPoints;

public static class GenresEndPoints
{
    public static RouteGroupBuilder MapGenresEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async(GameStoreContext dbContext) =>
            await dbContext.Genre
            .Select(genre => genre.ToDTO())
            .AsNoTracking()
            .ToListAsync());

        return group;
    }
}
