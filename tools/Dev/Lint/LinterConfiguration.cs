namespace Sustenance.Dev.Lint;

internal sealed record LinterConfiguration(IReadOnlyCollection<PackageManager> PackageManagers, IReadOnlyCollection<Linter> Linters)
{
}
