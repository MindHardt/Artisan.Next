namespace Artisan.Next.Client.Contracts.Files;

public record GetFilesRequest
{
    public string? PartialName { get; init; }
    public ManagedFileScope? RestrictToScope { get; init; }
    public int Page { get; init; } = 0;
    public int PageSize { get; init; } = 10;
}