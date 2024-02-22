using System.Security.Cryptography;
using Vogen;

namespace Artisan.Next.Data.Entities;

/// <summary>
/// A wrapper around <see cref="string"/> that encapsulates <see cref="SHA256"/> hash string that consists of
/// lowercase hexadecimal characters.
/// </summary>
/// <example>2e51a70ff807c3368eadebd3c223e96418d90ce22093bcfbde8a087ab96227d6</example>
[ValueObject<string>(Conversions.SystemTextJson | Conversions.EfCoreValueConverter)]
public readonly partial record struct HashString
{
    public const int Length = SHA256.HashSizeInBytes * 2;

    public static HashString FromHash(byte[] hash) => From(Convert.ToHexString(hash).ToLower());

    private static Validation Validate(string input) => 
        input.Length is Length && input.All(char.IsAsciiHexDigitLower) ? 
            Validation.Ok : 
            Validation.Invalid($"{nameof(HashString)}s must be exactly {Length} characters long and consist of hexadecimal characters");
}