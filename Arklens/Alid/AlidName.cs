using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

// ReSharper disable MemberCanBePrivate.Global

namespace Arklens.Alid;

/// <summary>
/// A wrapper around <see cref="string"/> that ensures that it is valid for use in <see cref="Alid"/>.
/// </summary>
/// <param name="Value"></param>
public readonly partial record struct AlidName(string Value)
{
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string ValidationRegexString = "[a-z_0-9]+";


    [GeneratedRegex(ValidationRegexString)]
    public static partial Regex ValidationRegex();

    /// <summary>
    /// Checks whether <paramref name="value"/> is a valid <see cref="AlidName"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsValid(string value) => ValidationRegex().IsMatch(value);

    /// <summary>
    /// The inner <see cref="string"/> representation of this <see cref="AlidName"/>.
    /// </summary>
    public string Value { get; } = IsValid(Value)
        ? Value
        : throw new ArgumentException($"Provided string is not a valid {nameof(AlidName)}.");

    public static implicit operator string(AlidName alidName) => alidName.Value;

    /// <summary>
    /// Returns an inner <see cref="Value"/> of this <see cref="AlidName"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        Value;

    /// <summary>
    /// Creates a new <see cref="AlidName"/> from <paramref name="value"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static AlidName Create(string value) => new(value);
}