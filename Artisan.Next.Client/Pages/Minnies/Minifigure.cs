using System.Web;

namespace Artisan.Next.Client.Pages.Minnies;

public record Minifigure
{
    public required string ImageBase64 { get; set; }
    public required string Name { get; set; }
    public string NormalizedName => HttpUtility.HtmlEncode(Name);
}