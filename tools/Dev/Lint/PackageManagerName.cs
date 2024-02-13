namespace Sustenance.Dev.Lint;

internal sealed record PackageManagerName(string Value)
{
    public override string ToString() => Value;
}
