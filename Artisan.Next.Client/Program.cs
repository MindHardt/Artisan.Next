using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Artisan.Next.Client;
using Artisan.Next.Client.JsInterop;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

builder.Services.AddScoped<DownloadJsInterop>();
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    options.WriteIndented = true;
    options.Converters.Add(new JsonStringEnumConverter());
});

await builder.Build().RunAsync();
