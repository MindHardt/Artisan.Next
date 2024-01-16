using Artisan.Next.Client;
using Artisan.Next.Client.Contracts;
using Artisan.Next.Data;
using Microsoft.EntityFrameworkCore;

namespace Artisan.Next.Handlers;

public class GetFilesHandler(
    IHttpContextAccessor httpContextAccessor,
    DataContext dataContext)
    : IRequestHandler<GetFilesRequest, IReadOnlyCollection<ManagedFileModel>>
{
    public async Task<IReadOnlyCollection<ManagedFileModel>> Handle(GetFilesRequest request, CancellationToken ct)
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

        return await query.OrderByDescending(x => x.DateUpdated)
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ManagedFileModel(
                x.UniqueName,
                x.OriginalName,
                x.MimeType,
                x.DateCreated,
                x.DateUpdated,
                x.Scope))
            .ToListAsync(ct);
    }
}