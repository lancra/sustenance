using Autofac;
using SimpleExec;
using Sustenance.Dev;
using Sustenance.Dev.Targets;

var container = DevStartup.BuildContainer();

var targets = new Bullseye.Targets();
foreach (var target in container.Resolve<IEnumerable<ITarget>>())
{
    target.Setup(targets);
}

await targets.RunAndExitAsync(args, messageOnly: ex => ex is ExitCodeException)
    .ConfigureAwait(false);
