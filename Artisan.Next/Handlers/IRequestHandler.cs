namespace Artisan.Next.Handlers;

public interface IRequestHandler<in TRequest, TResult>
{
    /// <summary>
    /// Handles <typeparamref name="TRequest"/>, producing <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task<TResult> Handle(TRequest request, CancellationToken ct = default);
}