namespace Sustenance.Dev.Targets.Build;

internal sealed class PublishTarget : ITarget
{
    public void Setup(Bullseye.Targets targets)
        => targets.Add(
            BuildTargets.Publish,
            "Publishes executable projects and dependencies to a folder for deployment.",
            dependsOn: [BuildTargets.Dotnet],
            forEach: [new ExecutableProject("api", "src/Api")],
            async project => await DotnetCli
                .RunAsync(
                    $"publish {project.Path}",
                    "--no-build",
                    $"--output {string.Format(null, ArtifactPaths.PublishedExecutableFormat, project.Name)}")
                .ConfigureAwait(false));
}
