using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class CreateGameDto(
   [Required][StringLength(50)] string Name,
    [Required][StringLength(50)] string Genre,
    [Range(0, 1000)] decimal Price,
    DateOnly ReleaseDate
);
