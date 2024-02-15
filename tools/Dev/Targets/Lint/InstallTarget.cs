using Sustenance.Dev.Lint;

namespace Sustenance.Dev.Targets.Lint;

internal sealed class InstallTarget : ITarget
{
    private static LinterConfiguration Configuration
    {
        get
        {
            var configurationResult = LinterConfigurationReader.Read();
            return configurationResult.IsValid
                ? configurationResult.Value!
                : throw new InvalidOperationException(configurationResult.Error);
        }
    }

    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            LintTargets.Install,
            "Installs configured static analysis tools on the current system.",
            async () => await LinterInstaller.InstallAsync(Configuration)
                .ConfigureAwait(false));
}
