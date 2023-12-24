using System.ComponentModel;

namespace Arklens.Alid;

/// <summary>
/// An entity that can be identified using <see cref="Alid"/>.
/// </summary>
public abstract record AlidEntity : ILocalizable
{
    /// <summary>
    /// The <see cref="Arklens.Alid.Alid"/> of this <see cref="AlidEntity"/>.
    /// </summary>
    public abstract Alid Alid { get; }

    /// <summary>
    /// Contains emoji representation of this <see cref="AlidEntity"/>.
    /// </summary>
    [Localizable(false)]
    public abstract string Emoji { get; }

    public abstract string LocalizedName { get; }
}