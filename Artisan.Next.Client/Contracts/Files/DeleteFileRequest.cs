namespace Artisan.Next.Client.Contracts.Files;

public record DeleteFileRequest
{
    public required string UniqueName { get; init; }
}