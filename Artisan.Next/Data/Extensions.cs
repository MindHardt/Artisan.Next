using Artisan.Next.Data.Entities;

namespace Artisan.Next.Data;

public static class Extensions
{
    /// <summary>
    /// Gets relative file location for this <see cref="ManagedFile"/>.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string GetLocation(this ManagedFile file)
        => $"/files/{file.UniqueName}";
}