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
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

builder.Services.AddRefitClient<IBackendClient>(sp => new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(
        sp.GetRequiredService<IOptions<JsonSerializerOptions>>().Value),
});

builder.Services.AddScoped<DownloadJsInterop>();
builder.Services.ConfigureJsonOptions();

await builder.Build().RunAsync();
