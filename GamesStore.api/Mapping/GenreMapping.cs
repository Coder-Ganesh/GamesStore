using GamesStore.api.Dtos;
using GamesStore.api.Entities;

namespace GamesStore.api.Mapping;

public static class GenreMapping
{
    public static GenreDTO ToDTO(this Genre genre)
    {
        return new GenreDTO(genre.Id,genre.Name);
    }
}
