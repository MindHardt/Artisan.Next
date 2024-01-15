using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Artisan.Next.Client.Pages;

public partial class Minifigures
{
    private List<Minifigure> _minnies = [];

    private async Task UploadMinifigureImage(InputFileChangeEventArgs e)
    {
        var minnieImage = await e.File.RequestImageFileAsync("jpeg", 512, 512);
        var minnieName = Path.GetFileNameWithoutExtension(minnieImage.Name);

        await using var imageStream = minnieImage.OpenReadStream(1024 * 1024 * 2);
        var base64Stream = new MemoryStream();
        await imageStream.CopyToAsync(base64Stream);

        var base64String = Convert.ToBase64String(base64Stream.ToArray());
        var minnie = new Minifigure
        {
            Name = minnieName,
            ImageBase64 = base64String
        };
        _minnies.Add(minnie);
    }

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

            if (index >= _minnies.Count)
            {
                return string.Empty;
            }
            var minnie = _minnies[index];

            return fieldType switch
            {
                "name" => minnie.NormalizedName,
                "avatar" => minnie.ImageBase64,
                _ => string.Empty
            };
        });
        return processed;
    }

    private async Task DownloadSvg()
        => await Download.DownloadAsync(PrepareSvg(), "minnies.svg");

    private async Task SetTemplate(Template template)
    {
        _currentTemplate = template;
        await LoadOriginalSvg();
    }

    private async Task LoadOriginalSvg()
    {
        _originalSvg = TransformOriginalSvg(await HttpClient.GetStringAsync(_currentTemplate.Url));
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

    private async Task ExportJson()
    {
        var images = _minnies
            .Select(x => x.ImageBase64)
            .Distinct()
            .ToArray();
        var minnies = _minnies
            .Select(x => new MinnieJsonModel(Array.IndexOf(images, x.ImageBase64), x.Name))
            .ToArray();
        var export = new MinniesSheetJsonModel(minnies, images);
        var jsonStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(jsonStream, export, JsonOptions.Value);
        jsonStream.Seek(0, SeekOrigin.Begin);

        await Download.DownloadAsync(jsonStream, "minnies.json");
    }

    private async Task ImportJson(InputFileChangeEventArgs e)
    {
        await using var jsonStream = e.File.OpenReadStream(1024 * 1024 * 20);
        var model = await JsonSerializer.DeserializeAsync<MinniesSheetJsonModel>(jsonStream, JsonOptions.Value);
        var minnies = model!.Minnies.Select(x => new Minifigure
        {
            ImageBase64 = model.Images[x.ImageIndex],
            Name = x.Name
        });
        _minnies.AddRange(minnies);
    }
}

public record Minifigure
{
    public required string ImageBase64 { get; set; }
    public required string Name { get; set; }
    public string NormalizedName => HttpUtility.HtmlEncode(Name);
}

public record MinnieJsonModel(int ImageIndex, string Name);
public record MinniesSheetJsonModel(MinnieJsonModel[] Minnies, string[] Images);