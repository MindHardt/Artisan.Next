using System.ComponentModel.DataAnnotations;

namespace Artisan.Next.Client.Contracts.Chits;

public record HealthModel
{
    [Range(1, int.MaxValue)]
    public required int MaxHealth { get; set; }
    [Range(1, int.MaxValue)]
    public required int CurrentHealth { get; set; }
}