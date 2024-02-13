namespace Sustenance.Dev.Lint;

internal sealed record ReporterOutputSegment(string Text, ConsoleColor? Color)
{
    public ReporterOutputSegment(string text)
        : this(text, default)
    {
    }
}
