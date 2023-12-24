namespace Arklens.Alid;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class AlidDomainAttribute(string domainName) : Attribute
{
    public string DomainName { get; } = domainName;
}