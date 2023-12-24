namespace Arklens.Alid;

public static class AlidExtensions
{
    /// <summary>
    /// Attempts to find <see cref="AlidEntity"/> of type <typeparamref name="TEntity"/> with
    /// <see cref="AlidEntity.Alid"/> equal to <paramref name="alid"/>.
    /// </summary>
    /// <param name="alidSearch"></param>
    /// <param name="alid"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static TEntity? Find<TEntity>(this IAlidSearch alidSearch, string alid)
        where TEntity : AlidEntity
        => alidSearch.Find(alid) as TEntity;
}