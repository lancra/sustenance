using System.Text;

namespace Sustenance.Dev;

internal static class DotnetCli
{
    private static readonly CompositeFormat ArgsFormat =
        CompositeFormat.Parse($"{{0}} --configuration Release --verbosity minimal --nologo{{1}}");

    public static async Task RunAsync(string command, params string[] args)
    {
        var additionalArgsString = string.Empty;
        if (args.Length != 0)
        {
            additionalArgsString = $" {string.Join(' ', args)}";
        }

        var argsString = string.Format(null, ArgsFormat, command, additionalArgsString);
        await SimpleExec.Command.RunAsync("dotnet", argsString)
            .ConfigureAwait(false);
    }
}
