using System.ComponentModel.DataAnnotations;

namespace TinyUrl.Features.TinyUrl.CreateUrl;

public record CreateTinyUrlRequest
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public string FullUrl { get; init; }
}