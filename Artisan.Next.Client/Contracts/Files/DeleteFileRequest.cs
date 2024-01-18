using System.ComponentModel.DataAnnotations;

namespace Artisan.Next.Client.Contracts.Files;

public record DeleteFileRequest
{
    [Required]
    public required string UniqueName { get; init; }
}