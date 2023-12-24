using System.Collections;
using System.Collections.Immutable;

namespace Arklens.Alid;

/// <summary>
/// A read-only collection of <see cref="AlidName"/>s.
/// </summary>
/// <param name="names"></param>
public readonly struct AlidNameCollection(IReadOnlyList<AlidName> names) : IReadOnlyList<AlidName>, IEquatable<AlidNameCollection>
{
    /// <summary>
    /// An instance of empty <see cref="AlidNameCollection"/>.
    /// </summary>
    public static AlidNameCollection Empty { get; } = new(ImmutableArray<AlidName>.Empty);

    /// <summary>
    /// Creates a new <see cref="AlidNameCollection"/> from <paramref name="stringValues"/>.
    /// </summary>
    /// <param name="stringValues"></param>
    /// <returns></returns>
    public static AlidNameCollection Create(IEnumerable<string> stringValues) =>
        Create(stringValues.Select(AlidName.Create));

    /// <inheritdoc cref="Create(System.Collections.Generic.IEnumerable{string})"/>
    public static AlidNameCollection Create(params string[] stringValues) =>
        Create((IEnumerable<string>)stringValues);

    /// <summary>
    /// Creates a new <see cref="AlidNameCollection"/> from <paramref name="alidNames"/>.
    /// </summary>
    /// <param name="alidNames"></param>
    /// <returns></returns>
    public static AlidNameCollection Create(IEnumerable<AlidName> alidNames) =>
        alidNames.ToImmutableArray() is { Length: > 0 } notEmptyArray
            ? new AlidNameCollection(notEmptyArray)
            : Empty;
    
    #region IList implementations

    public IEnumerator<AlidName> GetEnumerator() => 
        names.GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => 
        GetEnumerator();
    
    public int Count => 
        names.Count;
    
    public AlidName this[int index] => 
        names[index];

    #endregion

    public bool Equals(AlidNameCollection other) => 
        this.SequenceEqual(other);

    public override bool Equals(object? obj) =>
        obj is AlidNameCollection other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(names);

    public static bool operator ==(AlidNameCollection left, AlidNameCollection right) =>
        left.Equals(right);

    public static bool operator !=(AlidNameCollection left, AlidNameCollection right) =>
        left.Equals(right) is false;
}