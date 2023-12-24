using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Arklens.Alid;

/// <summary>
/// <para>
/// Alid (acronym of Arklens ID) is a way to uniform ids for everything in arklens code.
/// <see cref="Alid"/> is inspired by and is based on minecraft text ids.
/// <see cref="Alid"/>s are written in snake-case and consist of following parts:
/// </para>
/// <para>
/// ○ <see cref="Domains"/>, which describe the type of an entity. Entity must have at least one domain.
/// </para>
/// <para>
/// ○ <see cref="Name"/>, which must be unique per domain.
/// </para>
/// <para>
/// ○ <see cref="Modifiers"/>, which are responsible for variations within the type. They are optional in most cases.
/// </para>
/// <para>
/// Examples of valid <see cref="Alid"/>s:
/// </para>
/// <code>
/// spell:wizard:fireball
/// trait:expert+swimming
/// weapon:rapier+well_made+flexible
/// </code>
/// </summary>
public partial class Alid : IEquatable<Alid>, IParsable<Alid>
{
    /// <summary>
    /// Maximum allowed length of <see cref="Alid"/>.
    /// </summary>
    public const int MaxStringLength = 128;
    
    public const string ValidationRegexString = 
        $@"^(?<Domains>({AlidName.ValidationRegexString}:)+)(?<Name>#?{AlidName.ValidationRegexString})(?<Modifiers>(\+{AlidName.ValidationRegexString})*)$";
    
    [GeneratedRegex(ValidationRegexString)]
    public static partial Regex ValidationRegex();

    /// <summary>
    /// The default undefined <see cref="Alid"/> with <see cref="Text"/> equal to
    /// <code>
    /// alid:undefined
    /// </code>
    /// </summary>
    public static Alid Undefined { get; } = new(
        AlidNameCollection.Create("alid"), 
        AlidName.Create("undefined"),
        AlidNameCollection.Empty);
    
    /// <summary>
    /// The inner text value of <see cref="Alid"/>.
    /// </summary>
    [MaxLength(MaxStringLength)]
    public string Text { get; }
    public AlidNameCollection Domains { get; }
    public AlidName Name { get; }
    public AlidNameCollection Modifiers { get; }
    
    /// <summary>
    /// Indicates that this <see cref="Alid"/> refers to a group of values rather than individual value.
    /// </summary>
    public bool IsGroup { get; }

    public Alid(AlidNameCollection domains, AlidName name, AlidNameCollection modifiers, bool isGroup = false)
    {
        Domains = domains;
        Name = name;
        Modifiers = modifiers;
        IsGroup = isGroup;
        Text = BuildStringValue();
    }

    private string BuildStringValue()
    {
        StringBuilder sb = new(MaxStringLength);
        foreach (var domain in Domains)
        {
            sb.Append($"{domain}:");
        }

        if (IsGroup)
        {
            sb.Append('#');
        }

        sb.Append(Name);

        foreach (var modifier in Modifiers)
        {
            sb.Append($"+{modifier}");
        }

        if (sb.Length > MaxStringLength)
        {
            throw new InvalidOperationException($"{nameof(Alid)} cannot be longer than {MaxStringLength}.");
        }

        return sb.ToString();
    }

    public bool Equals(Alid? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Text == other.Text;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Alid)obj);
    }

    public override int GetHashCode() =>
        Text.GetHashCode();

    public static Alid Parse(string s, IFormatProvider? provider = null)
    {
        if (s.Length > MaxStringLength)
        {
            throw new OverflowException($"Input string exceeds maximum {nameof(Alid)} length of {MaxStringLength}");
        }
        
        if (ValidationRegex().Match(s) is not { Success: true } match)
        {
            throw new FormatException($"Input string does not match {nameof(Alid)}s {nameof(ValidationRegex)}");
        }

        return FromMatch(match);
    }

    /// <inheritdoc cref="TryParse(string?,System.IFormatProvider?,out Arklens.Alid.Alid)"/>
    public static bool TryParse(string? s, out Alid result) => TryParse(s, null, out result);
    
    public static bool TryParse(string? s, IFormatProvider? provider, out Alid result)
    {
        result = Undefined;

        if (s is not { Length: < MaxStringLength } || ValidationRegex().Match(s) is not { Success: true } match)
        {
            return false;
        }

        result = FromMatch(match);
        return true;
    }

    private static Alid FromMatch(Match match)
    {
        var domainsGroup = match.Groups["Domains"].Value;
        var domainStrings = domainsGroup.Split(':', StringSplitOptions.RemoveEmptyEntries);
        var domains = AlidNameCollection.Create(domainStrings);

        var nameGroup = match.Groups["Name"].Value;
        var isGroup = nameGroup.StartsWith('#');
        var name = AlidName.Create(nameGroup.TrimStart('#'));

        var modifiersGroup = match.Groups["Modifiers"].Value;
        var modifierStrings = modifiersGroup.Split('+', StringSplitOptions.RemoveEmptyEntries);
        var modifiers = AlidNameCollection.Create(modifierStrings);

        return new Alid(domains, name, modifiers, isGroup);
    }
}