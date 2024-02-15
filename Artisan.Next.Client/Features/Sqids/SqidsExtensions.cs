using Sqids;
// ReSharper disable MemberCanBePrivate.Global

namespace Artisan.Next.Client.Features.Sqids;

public static class SqidsExtensions
{
    /// <summary>
    /// Converts this <see cref="float"/>s to their binary representation
    /// and then encodes them as sqid.
    /// </summary>
    public static string EncodeFloat(this SqidsEncoder<int> encoder, IEnumerable<float> numbers)
        => encoder.Encode(numbers.Select(BitConverter.SingleToInt32Bits));
    
    /// <inheritdoc cref="EncodeFloat(SqidsEncoder{int},System.Collections.Generic.IEnumerable{float})"/>
    public static string EncodeFloat(this SqidsEncoder<int> encoder, params float[] numbers)
        => EncodeFloat(encoder, numbers.AsEnumerable());

    /// <summary>
    /// Converts this <see cref="double"/>s to their binary representation
    /// and then encodes them as sqid.
    /// </summary>
    public static string EncodeDouble(this SqidsEncoder<long> encoder, IEnumerable<double> numbers)
        => encoder.Encode(numbers.Select(BitConverter.DoubleToInt64Bits));

    /// <inheritdoc cref="EncodeDouble(SqidsEncoder{long},System.Collections.Generic.IEnumerable{double})"/>
    public static string EncodeDouble(this SqidsEncoder<long> encoder, params double[] numbers)
        => EncodeDouble(encoder, numbers.AsEnumerable());

    public static SqidId AsSqidId(this string sqid)
        => sqid;

    public static SqidId AsSqidId(this int value)
        => value;
}