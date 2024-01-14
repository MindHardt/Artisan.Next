namespace Artisan.Next.Client.Features.Maps;

public readonly record struct RectangularArea(Point BottomLeft, Point TopRight) : IArea
{
    public RectangularArea(float bottom, float left, float top, float right)
        : this((left, bottom), (right, top))
    { }

    public float Area { get; } =
        MathF.Abs(TopRight.X - BottomLeft.X) *
        MathF.Abs(TopRight.Y - BottomLeft.Y);

    public bool Contains(Point point) =>
        BottomLeft.X <= point.X && point.X <= TopRight.X &&
        BottomLeft.Y <= point.Y && point.Y <= TopRight.Y;

    public Point GetRandomPoint(Random? random = null)
    {
        random ??= Random.Shared;

        var xSpan = TopRight.X - BottomLeft.X;
        var ySpan = TopRight.Y - BottomLeft.Y;

        var xDelta = xSpan * random.NextSingle();
        var yDelta = ySpan * random.NextSingle();

        return (BottomLeft.X + xDelta, BottomLeft.Y + yDelta);
    }
}