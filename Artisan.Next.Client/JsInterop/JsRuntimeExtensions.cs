using Microsoft.JSInterop;

namespace Artisan.Next.Client.JsInterop;

public static class JsRuntimeExtensions
{
    /// <summary>
    /// The default javascript alert function that displays message in a floating window.
    /// </summary>
    /// <param name="js"></param>
    /// <param name="message">The additional info displayed.</param>
    /// <returns></returns>
    public static ValueTask AlertAsync(this IJSRuntime js, string message)
        => js.InvokeVoidAsync("alert", message);

    /// <summary>
    /// The default javascript confirm function that asks for user confirmation in a floating window.
    /// </summary>
    /// <param name="js"></param>
    /// <param name="message">The additional info displayed.</param>
    /// <returns><see langword="true"/> if user agrees, otherwise <see langword="false"/>.</returns>
    public static ValueTask<bool> ConfirmAsync(this IJSRuntime js, string message)
        => js.InvokeAsync<bool>("confirm", message);

    /// <summary>
    /// The default javascript prompt function that asks for user input in a floating window.
    /// </summary>
    /// <param name="js"></param>
    /// <param name="message">The additional info displayed.</param>
    /// <returns>A value provided by user.</returns>
    public static ValueTask<string> PromptAsync(this IJSRuntime js, string message)
        => js.InvokeAsync<string>("prompt", message);
}