using System.ComponentModel.DataAnnotations;

namespace Artisan.Next.Client.Contracts.Files;

public record PostFileRequest<TFile>
{
    [Required]
    public required TFile File { get; init; }
    [Required]
    public required ManagedFileScope Scope { get; init; }
}