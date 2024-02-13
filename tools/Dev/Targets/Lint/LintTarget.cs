using Sustenance.Dev.Lint;
using static SimpleExec.Command;

namespace Sustenance.Dev.Targets.Lint;

internal sealed class LintTarget : ITarget
{
    private static readonly bool FixLinterIssues = Environment.GetEnvironmentVariable("SUSTENANCE_FIX_LINT") == "1";

    private static IReadOnlyCollection<Linter> Linters
    {
        get
        {
            var configurationResult = LinterConfigurationReader.Read();
            return configurationResult.IsValid
                ? configurationResult.Value!.Linters
                : throw new InvalidOperationException(configurationResult.Error);
        }
    }

    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            LintTargets.Lint,
            "Flags stylistic and functional issues via static code analysis tools.",
            forEach: Linters,
            async linter => await RunAsync(linter.Name, FixLinterIssues ? linter.Fix ?? linter.Check : linter.Check)
                .ConfigureAwait(false));
}
