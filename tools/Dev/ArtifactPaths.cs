using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Sustenance.Dev;

[SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1203:Constants should appear before fields",
    Justification = "Static formats are grouped with their related constants.")]
internal static class ArtifactPaths
{
    public const string Root = "artifacts";

    public const string PublishedExecutables = $"{Root}/publish";
    public static readonly CompositeFormat PublishedExecutableFormat = CompositeFormat.Parse($"{PublishedExecutables}/{{0}}");

    public const string TestResults = $"{Root}/test-results";
    public static readonly CompositeFormat TestResultFormat = CompositeFormat.Parse($"{TestResults}/{{0}}");
}
