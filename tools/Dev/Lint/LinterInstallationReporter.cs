namespace Sustenance.Dev.Lint;

internal sealed class LinterInstallationReporter
{
    private readonly ReporterOutputSegment _prefixSegment;

    public LinterInstallationReporter(string? scope = default)
        => _prefixSegment = new ReporterOutputSegment(!string.IsNullOrEmpty(scope) ? $"{scope}: " : string.Empty, ConsoleColor.DarkGray);

    public void Write(string message)
        => Write(new ReporterOutputSegment(message));

    public void Write(params ReporterOutputSegment[] segments)
    {
        WriteSegment(_prefixSegment);
        foreach (var segment in segments)
        {
            WriteSegment(segment);
        }

        Console.WriteLine();
    }

    private static void WriteSegment(ReporterOutputSegment segment)
    {
        var priorColor = Console.ForegroundColor;
        var overrideColor = segment.Color is not null;
        if (overrideColor)
        {
            Console.ForegroundColor = segment.Color!.Value;
        }

        Console.Write(segment.Text);

        if (overrideColor)
        {
            Console.ForegroundColor = priorColor;
        }
    }
}
