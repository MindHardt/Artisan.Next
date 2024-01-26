namespace Artisan.Next.Client.Pages.Chits;

public record ModifierValue(string Emoji, int Value)
{
    public string Emoji { get; set; } = Emoji;
    public int Value { get; set; } = Value;
}