using Autofac;
using Sustenance.Dev.Targets;

namespace Sustenance.Dev;

internal static class DevStartup
{
    public static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterAssemblyTypes(typeof(DevStartup).Assembly)
            .AssignableTo<ITarget>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        return builder.Build();
    }
}
