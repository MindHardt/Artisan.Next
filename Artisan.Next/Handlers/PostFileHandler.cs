using Artisan.Next.Client;
using Artisan.Next.Client.Contracts;
using Artisan.Next.Services;

namespace Artisan.Next.Handlers;

public record PostFileRequest(
    IFormFile File,
    ManagedFileScope Scope);

public class PostFileHandler(
    IHttpContextAccessor httpContextAccessor,
    ManagedFileService service)
    : IRequestHandler<PostFileRequest, ManagedFileModel>
{
    public async Task<ManagedFileModel> Handle(PostFileRequest request, CancellationToken ct)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId()!.Value;
        var file = await service.SaveFile(request.File, userId, request.Scope, ct);

        return new ManagedFileModel(
            file.UniqueName,
            file.OriginalName,
            file.MimeType,
            file.DateCreated,
            file.DateUpdated,
            file.Scope);
    }
}