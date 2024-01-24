namespace Artisan.Next.Client.Contracts.Chits;

public record StatsModel
{
    public required int Fort { get; set; }
    public required int Reac { get; set; }
    public required int Will { get; set; }
    public required int Conc { get; set; }
    public required int Perc { get; set; }
    public required int Init { get; set; }
}