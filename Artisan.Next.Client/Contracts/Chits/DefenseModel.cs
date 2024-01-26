namespace Artisan.Next.Client.Contracts.Chits;

public record DefenseModel
{
    public required int Primary { get; set; }
    public required int Touch { get; set; }
    public required int Unaware { get; set; }
}