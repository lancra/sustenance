namespace Sustenance.Dev.Targets.Build;

internal sealed class CleanTarget : ITarget
{
    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            BuildTargets.Clean,
            "Cleans .NET build artifacts from prior executions.",
            async () => await DotnetCli.RunAsync("clean")
                .ConfigureAwait(false));
}
