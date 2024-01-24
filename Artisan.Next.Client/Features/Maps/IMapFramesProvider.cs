namespace Artisan.Next.Client.Features.Maps;

public interface IMapFramesProvider
{
    /// <summary>
    /// Gets a <see cref="Uri"/> of the provided <see cref="IArea"/>
    /// that is used to show it on the map.
    /// </summary>
    public Uri GetAreaDisplayUri(IArea area);
    /// <summary>
    /// Gets a street view of the provided <see cref="Point"/>.
    /// </summary>
    public Uri GetStreetViewUri(Point point);
    /// <summary>
    /// Gets a street view of a random point in the provided <see cref="IArea"/>.
    /// </summary>
    public Uri GetStreetViewUri(IArea area, Random? random = null) => GetStreetViewUri(area.GetRandomPoint(random));
    /// <summary>
    /// Gets a map view showing provided point on a map.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Uri GetPointDescriptionUri(Point point);
}