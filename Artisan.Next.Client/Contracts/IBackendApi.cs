using Artisan.Next.Client.Contracts.Files;
using Refit;

namespace Artisan.Next.Client.Contracts;

public interface IBackendApi
{
    [Get("/files")]
    public Task<ManagedFileModel[]> GetFiles(
        [Query] GetFilesRequest request, CancellationToken ct = default);

    [Multipart]
    [Post("/files")]
    protected Task<ManagedFileModel> PostFile(
        [AliasAs(nameof(PostFileRequest<int>.File))] StreamPart file,
        [AliasAs(nameof(PostFileRequest<int>.Scope))] string scope,
        CancellationToken ct = default);

    public Task<ManagedFileModel> PostFile(PostFileRequest<StreamPart> request, CancellationToken ct = default)
        => PostFile(request.File, request.Scope.ToString(), ct);

    [Delete("/files")]
    public Task<ManagedFileModel> DeleteFile(
        [Query] DeleteFileRequest request,
        CancellationToken ct = default);
}