namespace Artisan.Next.Handlers;

public static class DependencyInjection
{
    public static IServiceCollection AddHandlers(this IServiceCollection services) => services.Scan(scan =>
    {
        scan.FromAssembliesOf(typeof(DependencyInjection))
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime();
    });
}