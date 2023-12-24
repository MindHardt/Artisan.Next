using System.Reflection;

namespace Arklens.Alid;

/// <summary>
/// An implementation of <see cref="IAlidSearch"/> that uses reflection.
/// </summary>
public class ReflectiveAlidSearch : IAlidSearch
{
    private readonly Dictionary<Alid, AlidEntity> _lookup;

    public AlidEntity? Find(string alid) =>
        _lookup.GetValueOrDefault(Alid.Parse(alid));
}