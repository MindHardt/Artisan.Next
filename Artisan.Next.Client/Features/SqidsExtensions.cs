using Sqids;

namespace Artisan.Next.Client.Features;

public static class SqidsExtensions
{
    /// <summary>
    /// Adds default <see cref="SqidsEncoder{T}"/> for <see cref="int"/> and <see cref="long"/> types.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqidsEncoder(this IServiceCollection services,
        Action<SqidsOptions>? configureOptions = null)
    {
        if (configureOptions is not null)
        {
            var options = new SqidsOptions();
            configureOptions(options);

            services.AddScoped(_ => new SqidsEncoder<int>(options));
            services.AddScoped(_ => new SqidsEncoder<long>(options));
        }
        else
        {
            services.AddScoped(_ => new SqidsEncoder<int>());
            services.AddScoped(_ => new SqidsEncoder<long>());
        }

        return services;
    }
}