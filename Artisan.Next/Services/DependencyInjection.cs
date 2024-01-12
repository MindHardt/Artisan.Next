namespace Artisan.Next.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services.Scan(scan =>
        {
            scan.FromAssemblyOf<IService>()
                .AddClasses(c => c.AssignableTo<IService>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime();
        });
}