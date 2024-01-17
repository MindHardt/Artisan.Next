namespace Artisan.Next.Client.Contracts.Files;

public record PostFileRequest<TFile>
{
    public required TFile File { get; init; }
    public required ManagedFileScope Scope { get; init; }
}