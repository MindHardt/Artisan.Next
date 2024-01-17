using System.Text.Json;
using Artisan.Next.Client;
using Artisan.Next.Client.Contracts;
using Artisan.Next.Client.JsInterop;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

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

await builder.Build().RunAsync();