namespace Sustenance.Dev.Lint;

internal sealed class LinterConfigurationResult
{
    private LinterConfigurationResult(bool isValid, LinterConfiguration? value, string error)
    {
        IsValid = isValid;
        Value = value;
        Error = error;
    }

    public bool IsValid { get; }

    public LinterConfiguration? Value { get; }

    public string Error { get; }

    public static LinterConfigurationResult Success(LinterConfiguration value)
        => new(true, value, string.Empty);

    public static LinterConfigurationResult Failure(string error)
        => new(false, null, error);
}
