using System.Security.Claims;
using Artisan.Next.Client;
using Artisan.Next.Client.Contracts.Files;
using Artisan.Next.Data;
using Artisan.Next.Data.Entities;

namespace Artisan.Next.Handlers.Files;

public class PostFileHandler(
    ClaimsPrincipal user,
    DataContext dataContext,
    IWebHostEnvironment hostEnvironment)
    : IRequestHandler<PostFileRequest<IFormFile>, ManagedFileModel>
{
    public async Task<ManagedFileModel> Handle(PostFileRequest<IFormFile> request, CancellationToken ct = default)
    {
        var userId = user.GetUserId();
        await using var data = request.File.OpenReadStream();

        var uniqueName = GetUniqueName(request.File.FileName);
        await using var fs = File.Create($"{hostEnvironment.WebRootPath}/files/{uniqueName}");
        await data.CopyToAsync(fs, ct);

        var now = DateTimeOffset.UtcNow;
        var file = new ManagedFile
        {
            UniqueName = uniqueName,
            OriginalName = request.File.FileName,
            MimeType = request.File.ContentType,
            Scope = request.Scope,
            OwnerId = userId,
            DateCreated = now,
            DateUpdated = now
        };
        dataContext.Files.Add(file);
        await dataContext.SaveChangesAsync(ct);

        return new ManagedFileModel
        {
            UniqueName = file.UniqueName,
            OriginalName = file.OriginalName,
            MimeType = file.MimeType,
            DateCreated = file.DateCreated,
            DateUpdated = file.DateUpdated,
            Scope = file.Scope
        };
    }

    private static string GetUniqueName(string originalName)
        => $"{Path.GetRandomFileName()}{Path.GetExtension(originalName)}";
}