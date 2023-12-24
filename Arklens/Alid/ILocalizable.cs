using System.ComponentModel;

namespace Arklens.Alid;

/// <summary>
/// Exposes method for localized version of <see cref="object.ToString"/>.
/// </summary>
public interface ILocalizable
{
    /// <summary>
    /// Contains localized <see cref="string"/> representation of current <see cref="ILocalizable"/>.
    /// </summary>
    /// <returns></returns>
    [Localizable(true)]
    public string LocalizedName { get; }
}