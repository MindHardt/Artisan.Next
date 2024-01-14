﻿namespace Artisan.Next.Client.Features.Maps;

public readonly record struct CircularArea(Point Center, float Radius) : IArea
{
    public float Area { get; } = MathF.PI * Radius * Radius;

    public bool Contains(Point point)
        => Center.DistanceTo(point) <= Radius;

    public Point GetRandomPoint(Random? random = null)
    {
        random ??= Random.Shared;

        var angle = MathF.Tau * random.NextSingle();

        var xDelta = random.NextSingle() * Radius * MathF.Cos(angle);
        var yDelta = random.NextSingle() * Radius * MathF.Sin(angle);

        return (Center.X + xDelta, Center.Y + yDelta);
    }
}