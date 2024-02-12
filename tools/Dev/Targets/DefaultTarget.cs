namespace Sustenance.Dev.Targets;

internal sealed class DefaultTarget : ITarget
{
    public void Setup(Bullseye.Targets targets) => targets.Add("default", () => { });
}
