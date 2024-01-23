namespace Artisan.Next.Client.Contracts.Chits;

public record CharacteristicsModel
{
    public required int Str { get; set; }
    public required int Dex { get; set; }
    public required int Con { get; set; }
    public required int Int { get; set; }
    public required int Wis { get; set; }
    public required int Cha { get; set; }
}