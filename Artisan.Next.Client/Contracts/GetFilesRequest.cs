namespace Artisan.Next.Client.Contracts;

public record GetFilesRequest(
    string? PartialName = null,
    ManagedFileScope? RestrictToScope = null,
    int Page = 0,
    int PageSize = 20);

public record ManagedFileModel(
    string UniqueName,
    string OriginalName,
    string MimeType,
    DateTimeOffset DateCreated,
    DateTimeOffset DateUpdated,
    ManagedFileScope Scope);

public enum ManagedFileScope
{
    Unknown = 0,
    Avatar = 1,
    MinniesSheet = 2
}