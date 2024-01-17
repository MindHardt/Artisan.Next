using Artisan.Next.Client;
using Artisan.Next.Client.Contracts.Files;
using Artisan.Next.Data;
using Microsoft.EntityFrameworkCore;

namespace Artisan.Next.Handlers.Files;

public class GetFilesHandler(
    IHttpContextAccessor httpContextAccessor,
    DataContext dataContext)
    : IRequestHandler<GetFilesRequest, IReadOnlyCollection<ManagedFileModel>>
{
    public async Task<IReadOnlyCollection<ManagedFileModel>> Handle(GetFilesRequest request, CancellationToken ct = default)
    {
        var userId = httpContextAccessor.HttpContext?.User.GetUserId()!.Value;
        var query = dataContext.Files
            .Where(x => x.OwnerId == userId);

        if (request.RestrictToScope is { } scope)
        {
            query = query.Where(x => x.Scope == scope);
        }
        if (request.PartialName is { } partialName)
        {
            query = query.Where(x => EF.Functions.ILike(x.OriginalName, $"%{partialName}%"));
        }

        var result = await query.OrderByDescending(x => x.DateUpdated)
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ManagedFileModel
            {
                UniqueName = x.UniqueName,
                OriginalName = x.OriginalName,
                MimeType = x.MimeType,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                Scope = x.Scope
            })
            .ToListAsync(ct);

        return result;
    }
}