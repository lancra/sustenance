using SimpleExec;
using static SimpleExec.Command;

namespace Sustenance.Dev.Lint;

internal static class LinterInstaller
{
    public static async Task InstallAsync(LinterConfiguration configuration)
    {
        var platform = GetPlatform();
        var installationResults = new Dictionary<string, LinterInstallationResult>();
        var packageManagers = configuration.PackageManagers.ToDictionary(pm => pm.Name);

        foreach (var linter in configuration.Linters)
        {
            var reporter = new LinterInstallationReporter(linter.Name);
            reporter.Write("Starting installation.");

            reporter.Write("Performing installation verification.");
            var hasLinter = await HasExecutable(linter.Name)
                .ConfigureAwait(false);
            if (hasLinter)
            {
                reporter.Write("Installation has been verified.");
                installationResults.Add(linter.Name, LinterInstallationResult.Verified);
                continue;
            }

            foreach (var package in linter.Packages)
            {
                reporter.Write($"Found a package for {package.Source}.");

                if (!packageManagers.TryGetValue(package.Source, out var packageManager))
                {
                    reporter.Write($"The usage of {package.Source} has not been configured.");
                    continue;
                }

                var packageManagerSupported = packageManager.Platforms.Contains(platform);
                if (!packageManagerSupported)
                {
                    reporter.Write($"The usage of {package.Source} is not supported on {platform}.");
                    continue;
                }

                reporter.Write($"Performing installation verification for {package.Source}.");
                var hasPackageManager = await HasExecutable(packageManager.Name.Value)
                    .ConfigureAwait(false);
                if (!hasPackageManager)
                {
                    var sourceText = !string.IsNullOrEmpty(packageManager.Source) ? $"via {packageManager.Source}" : string.Empty;
                    reporter.Write($"Unable to verify {package.Source}. " +
                        $"Please add it to the PTAH or download it{sourceText} at {packageManager.Url}.");
                    continue;
                }

                reporter.Write($"Performing installation of {package.Source}.");
                var installationSuccessful = await ExecuteInstallation(packageManager, package)
                    .ConfigureAwait(false);
                var installationResult = installationSuccessful ? LinterInstallationResult.Succeeded : LinterInstallationResult.Failed;
                var installationResultText = installationResult.ToString()
                    .ToLowerInvariant();

                reporter.Write($"Installation using {package.Source} has {installationResultText}.");
                installationResults.Add(linter.Name, installationResult);
                break;
            }

            if (installationResults.TryAdd(linter.Name, LinterInstallationResult.Skipped))
            {
                reporter.Write("Installation has been skipped.");
            }
        }

        var aggregateReporter = new LinterInstallationReporter();
        var maxLinterNameLength = configuration.Linters.Max(linter => linter.Name.Length);
        foreach (var linter in configuration.Linters)
        {
            var installationResult = installationResults[linter.Name];
            var installationResultColor = installationResult is LinterInstallationResult.Succeeded or LinterInstallationResult.Verified
                ? ConsoleColor.DarkGreen
                : ConsoleColor.DarkRed;

            aggregateReporter.Write(
                new(linter.Name.PadRight(maxLinterNameLength + 1, ' ')),
                new(installationResult.ToString(), installationResultColor));
        }
    }

    private static async Task<bool> ExecuteInstallation(PackageManager packageManager, LinterPackage package)
    {
        var installCommand = string.Format(packageManager.Install, package.Name);
        try
        {
            await RunAsync(packageManager.Name.Value, installCommand)
                .ConfigureAwait(false);
            return true;
        }
        catch (ExitCodeException)
        {
            return false;
        }
    }

    private static PackageManagerPlatform GetPlatform()
    {
        if (OperatingSystem.IsWindows())
        {
            return PackageManagerPlatform.Windows;
        }
        else if (OperatingSystem.IsMacOS())
        {
            return PackageManagerPlatform.MacOS;
        }
        else
        {
            return PackageManagerPlatform.Linux;
        }
    }

    private static async Task<bool> HasExecutable(string name)
    {
        var (output, error) = await ReadAsync(
            "pwsh",
            $"-NoProfile -Command \"$null -ne (Get-Command -Name {name} -ErrorAction SilentlyContinue)\"")
            .ConfigureAwait(false);
        var exists = bool.Parse(output);
        return exists;
    }
}
