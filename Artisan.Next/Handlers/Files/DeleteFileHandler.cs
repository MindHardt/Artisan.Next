using System.Security.Claims;
using Artisan.Next.Client;
using Artisan.Next.Client.Contracts.Files;
using Artisan.Next.Data;
using Microsoft.EntityFrameworkCore;

namespace Artisan.Next.Handlers.Files;

public class DeleteFileHandler(
    ClaimsPrincipal user,
    IWebHostEnvironment hostEnvironment,
    DataContext dataContext)
    : IRequestHandler<DeleteFileRequest, ManagedFileModel>
{
    public async Task<ManagedFileModel> Handle(DeleteFileRequest request, CancellationToken ct = default)
    {
        var userId = user.GetUserId();
        var file = await dataContext.Files
            .FirstOrDefaultAsync(x => x.UniqueName == request.UniqueName, ct);

        if (file is null)
        {
            throw new BadHttpRequestException("File is not found");
        }

        if (file.OwnerId != userId)
        {
            throw new BadHttpRequestException("You do not have access to that file");
        }

        dataContext.Files.Remove(file);
        await dataContext.SaveChangesAsync(ct);
        File.Delete($"{hostEnvironment.WebRootPath}/files/{file.UniqueName}");

        return new ManagedFileModel
        {
            UniqueName = file.UniqueName,
            OriginalName = file.OriginalName,
            MimeType = file.MimeType,
            DateCreated = file.DateCreated,
            DateUpdated = file.DateUpdated,
            Scope = file.Scope,
            Hash = file.Hash.Value
        };
    }
}