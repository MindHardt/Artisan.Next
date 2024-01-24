using System.Text.Json;
using Arklens.Next.Core;
using Artisan.Next.Client;
using Artisan.Next.Client.Contracts;
using Artisan.Next.Client.Features;
using Artisan.Next.Client.Features.Maps;
using Artisan.Next.Client.JsInterop;
using Bogus;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddMemoryCache();
builder.Services.AddYandexFrames();
builder.Services.AddSqidsEncoder(options =>
{
    options.Alphabet = "abcdefghijklmnopqrstuvwxyz";
    options.MinLength = 6;
});

var backendUri = new Uri(builder.HostEnvironment.BaseAddress);

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = backendUri
});
builder.Services.AddRefitClient<IBackendApi>(sp => new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(
        sp.GetRequiredService<IOptions<JsonSerializerOptions>>().Value)
})
.ConfigureHttpClient(client =>
{
    client.BaseAddress = backendUri;
});

builder.Services.AddScoped<DownloadJsInterop>();
builder.Services.ConfigureJsonOptions();
builder.Services.AddScoped(_ => new Faker("ru"));

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Loaded {Count} AlidEntities: {List}",
    AlidEntity.AllValues.Count,
    AlidEntity.AllValues.OrderBy(x => x.Alid.Text).Select(x => $"\n{x.Alid.Text}"));

await app.RunAsync();