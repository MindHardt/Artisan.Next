using Artisan.Next.Client.Features.Maps;

namespace Artisan.Next.Tests;

public class AreaTests
{
    private static readonly Random Random = new(123);
    public static readonly object[][] Areas =
    [
        [new RectangularArea(-2, -1, 2, 1)],
        [new CircularArea((0, 0), 2)],
        [new EllipsoidArea((0, 0), 2, 1)]
    ];

    [Theory]
    [MemberData(nameof(Areas))]
    public void TestRandomPointsInArea(IArea area)
    {
        for (var i = 0; i < 100_000; i++)
        {
            var randomPoint = area.GetRandomPoint(Random);
            Assert.True(area.Contains(randomPoint));
        }
    }
}