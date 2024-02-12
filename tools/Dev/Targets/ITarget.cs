namespace Sustenance.Dev.Targets;

/// <summary>
/// Represents a target task execution.
/// </summary>
internal interface ITarget
{
    /// <summary>
    /// Performs setup necessary to include the target into a collection.
    /// </summary>
    /// <param name="targets">The target collection to modify.</param>
    void Setup(Bullseye.Targets targets);
}
