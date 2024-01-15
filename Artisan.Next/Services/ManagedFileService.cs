using Artisan.Next.Data;
using Artisan.Next.Data.Entities;

namespace Artisan.Next.Services;

public class ManagedFileService(IWebHostEnvironment hostEnvironment, DataContext dataContext) : IService
{
    /// <summary>
    /// Saves file to the file storage.
    /// </summary>
    /// <param name="data">File contents. This method doesn't dispose this <see cref="Stream"/>.</param>
    /// <param name="fileName">Original file name.</param>
    /// <param name="mimeType">File's mime type.</param>
    /// <param name="ownerId"></param>
    /// <param name="scope"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<ManagedFile> SaveFile(Stream data, string fileName, string mimeType, Guid? ownerId, ManagedFileScope scope = ManagedFileScope.Unknown, CancellationToken ct = default)
    {
        var uniqueName = GetUniqueName(fileName);
        await using var fs = File.Create($"{hostEnvironment.WebRootPath}/files/{uniqueName}");
        await data.CopyToAsync(fs, ct);

        var managedFile = new ManagedFile
        {
            UniqueName = uniqueName,
            OriginalName = fileName,
            MimeType = mimeType,
            Scope = scope
        };
        dataContext.Files.Add(managedFile);
        await dataContext.SaveChangesAsync(ct);

        return managedFile;
    }

    /// <summary>
    /// Saves <paramref name="file"/> to the file storage. This method uses
    /// <see cref="IFormFile.OpenReadStream"/> and disposes opened stream.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="ownerId"></param>
    /// <param name="scope"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<ManagedFile> SaveFile(IFormFile file, Guid? ownerId, ManagedFileScope scope = ManagedFileScope.Unknown, CancellationToken ct = default)
    {
        await using var stream = file.OpenReadStream();
        return await SaveFile(stream, file.FileName, file.ContentType, ownerId, scope, ct);
    }

    private static string GetUniqueName(string originalName)
        => $"{Path.GetRandomFileName()}{Path.GetExtension(originalName)}";
}