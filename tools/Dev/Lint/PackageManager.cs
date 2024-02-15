namespace Sustenance.Dev.Lint;

internal sealed record PackageManager(PackageManagerName Name, Uri Url, string Install, IReadOnlyCollection<PackageManagerPlatform> Platforms)
{
    public string? Source { get; init; }
}
