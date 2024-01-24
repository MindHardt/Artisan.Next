using Arklens.Next.Entities;

namespace Artisan.Next.Client.Contracts.Chits;

public record MonsterChitModel
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required HealthModel Health { get; set; }
    public required CharacteristicsModel Characteristics { get; set; }
    public required StatsModel Stats { get; set; }
    public required string ImageUrl { get; set; }
    public required Alignment Alignment { get; set; }
}