using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Artisan.Next.Client.JsInterop;

public abstract class JsInteropBase : IAsyncDisposable
{
    /// <summary>
    /// The path of the .js file from the local wwwroot.
    /// </summary>
    /// <example>download.js</example>
    protected abstract string JsFilePath { get; }

    /// <summary>
    /// The underlying <see cref="IJSRuntime"/> for using
    /// default js functions.
    /// </summary>
    private IJSRuntime Runtime { get; }

    /// <summary>
    /// The underlying <see cref="IJSRuntime"/> object, wrapped for lazy evaluation.
    /// </summary>
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    /// <summary>
    /// Gets the imported javascript module.
    /// </summary>
    /// <returns></returns>
    protected Task<IJSObjectReference> GetModuleAsync() => _moduleTask.Value;
    
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    protected JsInteropBase(IJSRuntime jsRuntime)
    {
        Runtime = jsRuntime;
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() =>
            Runtime.InvokeAsync<IJSObjectReference>("import", JsFilePath).AsTask());
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}