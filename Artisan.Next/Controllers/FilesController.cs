using System.Net.Mime;
using Artisan.Next.Client.Contracts;
using Artisan.Next.Handlers;
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
        [FromForm] PostFileRequest request,
        [FromServices] PostFileHandler handler,
        CancellationToken ct)
        => await handler.Handle(request, ct);
}