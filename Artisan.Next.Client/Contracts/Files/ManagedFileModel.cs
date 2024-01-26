namespace Artisan.Next.Client.Contracts.Files;

public record ManagedFileModel
{
    public required string UniqueName { get; init; }
    public required string OriginalName { get; init; }
    public required string MimeType { get; init; }
    public required DateTimeOffset DateCreated { get; init; }
    public required DateTimeOffset DateUpdated { get; init; }
    public required ManagedFileScope Scope { get; init; }
}

public enum ManagedFileScope
{
    Unknown = 0,
    Avatar = 1,
    MinniesSheet = 2,
    MonsterChit = 3
}