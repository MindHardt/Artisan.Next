using Artisan.Next.Client.Features.Maps;

namespace Artisan.Next.Client.Contracts.Maps;

public record SavedMapAreaModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required IArea Area { get; set; }
}