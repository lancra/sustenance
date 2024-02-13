namespace Sustenance.Dev;

internal sealed record TestSuite(string Name, string Description, TestProject[] Projects)
{
    public override string ToString() => Name;
}
