using System.Net.Mime;
using Artisan.Next.Client.Contracts.Files;
using Artisan.Next.Handlers.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artisan.Next.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class FilesController : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyCollection<ManagedFileModel>> GetFiles(
        [FromQuery] GetFilesRequest request,
        [FromServices] GetFilesHandler handler,
        CancellationToken ct)
        => await handler.Handle(request, ct);

    [HttpPost]
    public async Task<ManagedFileModel> PostFile(
        [FromForm] PostFileRequest<IFormFile> request,
        [FromServices] PostFileHandler handler,
        CancellationToken ct)
        => await handler.Handle(request, ct);

    [HttpDelete]
    public async Task<ManagedFileModel> DeleteFile(
        [FromQuery] DeleteFileRequest request,
        [FromServices] DeleteFileHandler handler,
        CancellationToken ct)
        => await handler.Handle(request, ct);
}