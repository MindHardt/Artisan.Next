using System.Text.RegularExpressions;

namespace Artisan.Next.Client.Pages.Minnies;

public partial class SvgRender
{
    private static string TransformOriginalSvg(string svg)
    {
        var i = 1;
        return DefaultPortraitRegex.Replace(svg, _ => $"${{avatar_{i++}}}");
    }

    private string PrepareSvg()
    {
        var processed = PreparationRegex().Replace(_originalSvg!, match =>
        {
            var nameGroup = match.Groups["Name"].Value;

            var parts = nameGroup.Split('_');
            var (fieldType, index) = (parts[0], int.Parse(parts[1]) - 1);

            if (index >= Minnies.Count)
            {
                return string.Empty;
            }
            var minnie = Minnies[index];

            return fieldType switch
            {
                "name" => minnie.NormalizedName,
                "avatar" => minnie.ImageBase64,
                _ => string.Empty
            };
        });
        return processed;
    }

    [GeneratedRegex(@"\$\{(?<Name>[a-z]+_[0-9]+)\}")]
    private static partial Regex PreparationRegex();
    private static Regex DefaultPortraitRegex => new(Regex.Escape(DefaultPortraitBase64));

    private const string DefaultPortraitBase64 = // 1 pixel of 0xFFFFFF in jpg
        "/9j/4AAQSkZJRgABAQEAYABgAAD/4QBoRXhpZgAATU0AKgAAAAgABAEaAAUAAAABAAAAPgEbAAUAAAABAAAARgEoAAMAAAABAAIAAAExAAIAAA" +
        "ARAAAATgAAAAAAAABgAAAAAQAAAGAAAAABcGFpbnQubmV0IDUuMC4xMQAA/9sAQwABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEB" +
        "AQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEB/9sAQwEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQ" +
        "EBAQEBAQEBAQEBAQEBAQEB/8AAEQgAAQABAwESAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMF" +
        "BQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZG" +
        "VmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX2" +
        "9/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCka" +
        "GxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZ" +
        "mqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A/v4ooA//2Q==";
}