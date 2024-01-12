using System.Security.Claims;
using Artisan.Next.Client.Models;

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
}