using System.ComponentModel.DataAnnotations;

namespace GamesStore.api.Dtos;

public record class CreateGameDto(
    [Required][StringLength(50)]string Name,
    [Required]int GenreId,
    [Range(1,100)]decimal Price,
    DateOnly ReleaseDate);
