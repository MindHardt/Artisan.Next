using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Artisan.Next.Client.Contracts.Files;
using Artisan.Next.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Artisan.Next.Client;

public static class Extensions
{
    /// <summary>
    /// Gets managed server file path.
    /// </summary>
    /// <param name="managedFilePath"></param>
    /// <returns></returns>
    public static string GetManagedFilePath(this string managedFilePath)
        => $"files/{managedFilePath}";

    public static string? GetAvatarPath(this ClaimsPrincipal principal)
        => principal.FindFirst(nameof(UserModel.AvatarUrl))?.Value.GetManagedFilePath();

    /// <summary>
    /// Gets <paramref name="principal"/>s id as <see cref="Guid"/>.
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public static Guid? GetUserId(this ClaimsPrincipal? principal) =>
        Guid.TryParse(principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid id)
            ? id
            : null;

    public static string GetPath(this ManagedFileModel file)
        => file.UniqueName.GetManagedFilePath();

    public static IServiceCollection ConfigureJsonOptions(this IServiceCollection services) =>
        services.Configure<JsonSerializerOptions>(SetDefaults);

    public static void SetDefaults(this JsonSerializerOptions options)
    {
        options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.PropertyNameCaseInsensitive = true;
        options.Converters.Add(new JsonStringEnumConverter());
    }

    [DoesNotReturn]
    public static void NavigateToLogin(this NavigationManager navManager)
        => navManager.NavigateTo($"Account/Login?returnUrl={Uri.EscapeDataString(navManager.Uri.Replace("http", "https"))}", forceLoad: true);
}