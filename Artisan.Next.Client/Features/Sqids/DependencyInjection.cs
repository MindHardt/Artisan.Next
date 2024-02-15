using System.Numerics;
using Sqids;

namespace Artisan.Next.Client.Features.Sqids;

public static class DependencyInjection
{
    /// <summary>
    /// Adds default <see cref="SqidsEncoder{T}"/> for <see cref="int"/> and <see cref="long"/> types.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqidsEncoders(this IServiceCollection services,
        Action<SqidsOptions>? configureOptions = null) => services
        .AddSqidsEncoder<int>()
        .AddSqidsEncoder<long>();

    public static IServiceCollection AddSqidsEncoder<T>(this IServiceCollection services,
        Action<SqidsOptions>? configureOptions = null)
        where T : unmanaged, IBinaryInteger<T>, IMinMaxValue<T>
    {
        return services.AddScoped(sp =>
        {
            var options = sp.GetService<SqidsOptions>();
            if (options is null)
            {
                return new SqidsEncoder<T>();
            }

            configureOptions?.Invoke(options);
            return new SqidsEncoder<T>(options);
        });
    }
}