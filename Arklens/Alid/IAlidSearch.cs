namespace Arklens.Alid;

public interface IAlidSearch
{
    /// <summary>
    /// Attempts to find <see cref="AlidEntity"/> with <see cref="AlidEntity.Alid"/> equal to <see cref="alid"/>.
    /// </summary>
    /// <param name="alid"></param>
    /// <returns></returns>
    public AlidEntity? Find(string alid);
}