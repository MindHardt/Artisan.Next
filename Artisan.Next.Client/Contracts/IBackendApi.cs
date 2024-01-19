using Artisan.Next.Client.Contracts.Files;
using Refit;

namespace Artisan.Next.Client.Contracts;

public interface IBackendApi
{
    [Get($"/files/{{{nameof(fileName)}}}")]
    public Task<string> ReadFileAsString(
        string fileName,
        CancellationToken ct = default);

    public Task<string> ReadFileAsString(
        ManagedFileModel file,
        CancellationToken ct = default)
        => ReadFileAsString(file.UniqueName, ct);


    [Get($"/files/{{{nameof(fileName)}}}")]
    public Task<Stream> ReadFile(
        string fileName,
        CancellationToken ct = default);

    public Task<Stream> ReadFile(
        ManagedFileModel file,
        CancellationToken ct = default) => ReadFile(file.UniqueName, ct);

    [Get("/files")]
    public Task<ManagedFileModel[]> GetFiles(
        [Query] GetFilesRequest request,
        CancellationToken ct = default);

    [Multipart]
    [Post("/files")]
    protected Task<ManagedFileModel> PostFile(
        [AliasAs(nameof(PostFileRequest<StreamPart>.File))] StreamPart file,
        [AliasAs(nameof(PostFileRequest<StreamPart>.Scope))] string scope,
        CancellationToken ct = default);


    public Task<ManagedFileModel> PostFile(PostFileRequest<StreamPart> request, CancellationToken ct = default)
    {
        var file = new StreamPart(request.File.Value, request.File.FileName, request.File.ContentType,
            nameof(request.File));

        return PostFile(file, request.Scope.ToString(), ct);
    }

    [Delete("/files")]
    public Task<ManagedFileModel> DeleteFile(
        [Query] DeleteFileRequest request,
        CancellationToken ct = default);
}