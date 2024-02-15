using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Sqids;

namespace Artisan.Next.Client.Features.Sqids;

public class SqidId
{
    /// <summary>
    /// The <see cref="int"/> value of this <see cref="SqidId"/>.
    /// It may be null if this instance was created from <see cref="Sqid"/>
    /// and no encoder was provided.
    /// </summary>
    public int? Value { get; private set; }
    /// <summary>
    /// The <see cref="string"/> sqid of this <see cref="SqidId"/>.
    /// It may be null if this instance was created from <see cref="Sqid"/>
    /// and no encoder was provided.
    /// </summary>
    public string? Sqid { get; private set; }

    /// <summary>
    /// Creates a new <see cref="SqidId"/> from <see cref="string"/>
    /// sqid and optionally calculates <see cref="Value"/>
    /// if <paramref name="encoder"/> is provided.
    /// </summary>
    public SqidId(int value, SqidsEncoder<int>? encoder)
    {
        Value = value;
        Sqid = encoder?.Encode(value);
    }
    public static implicit operator SqidId(int value) => new(value, null);

    /// <summary>
    /// Creates a new <see cref="SqidId"/> from <see cref="int"/>
    /// value and optionally calculates <see cref="Sqid"/>
    /// if <paramref name="encoder"/> is provided.
    /// </summary>
    public SqidId(string sqid, SqidsEncoder<int>? encoder)
    {
        Sqid = sqid;
        Value = encoder?.Decode(sqid.AsSpan())[0];
    }
    public static implicit operator SqidId(string sqid) => new(sqid, null);

    /// <summary>
    /// Sets all properties of this <see cref="SqidId"/>
    /// using provided <see cref="SqidsEncoder{T}"/>.
    /// </summary>
    /// <returns>Reference to the same instance.</returns>
    public SqidId Populate(SqidsEncoder<int> encoder)
    {
        (Sqid, Value) = (Sqid, Value) switch
        {
            (null, not null) => (encoder.Encode(Value.Value), Value),
            (not null, null) => (Sqid, encoder.Decode(Sqid)[0]),
            (null, null) => throw new UnreachableException(),
            (not null, not null) => (Sqid, Value)
        };
        return this;
    }
}