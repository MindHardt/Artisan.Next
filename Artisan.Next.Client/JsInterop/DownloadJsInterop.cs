using System.Text;
using Microsoft.JSInterop;

namespace Artisan.Next.Client.JsInterop;

public class DownloadJsInterop(IJSRuntime jsRuntime) : JsInteropBase(jsRuntime)
{
    protected override string JsFilePath => "js/download.js";

    public ValueTask DownloadAsync(string content, string fileName)
        => DownloadAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)), fileName);

    public async ValueTask DownloadAsync(Stream stream, string fileName)
    {
        var module = await GetModuleAsync();
        DotNetStreamReference streamRef = new(stream);

        await module.InvokeVoidAsync("downloadFile", streamRef, fileName);
    }
}