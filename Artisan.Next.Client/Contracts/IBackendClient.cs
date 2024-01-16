using Refit;

namespace Artisan.Next.Client.Contracts;

public interface IBackendClient
{
    [Get("/files")]
    public Task<IReadOnlyCollection<ManagedFileModel>> GetManagedFiles(
        [Query] GetFilesRequest request, CancellationToken ct = default);

    [Multipart]
    [Post("/files")]
    public Task<ManagedFileModel> SaveManagedFile(
         Stream file, ManagedFileScope scope, CancellationToken ct = default);
}