using System.Globalization;

namespace Artisan.Next.Client.Features.Maps;

public class YandexMapFramesProvider : IMapFramesProvider
{
    public Uri GetAreaDisplayUri(IArea area)
    {
        throw new NotImplementedException();
    }

    private const string StreetViewUrl =
        "https://yandex.ru/map-widget/v1/?ll={0}%2C{1}&panorama%5Bdirection%5D=0.000000%2C0.000000&panorama%5Bpoint%5D={2}%2C{3}&z=100";
    public Uri GetStreetViewUri(Point point)
        => new(string.Format(StreetViewUrl,
            Format(point.X), Format(point.Y), Format(point.X), Format(point.Y)));

    public Uri GetPointDescriptionUri(Point point)
    {
        throw new NotImplementedException();
    }

    private static string Format(float coordinate) => coordinate.ToString("0.000000", CultureInfo.InvariantCulture);
}

public static class DependencyInjection
{
    public static IServiceCollection AddYandexFrames(this IServiceCollection services) => services
        .AddScoped<IMapFramesProvider, YandexMapFramesProvider>()
        .AddScoped<YandexMapFramesProvider>();
}