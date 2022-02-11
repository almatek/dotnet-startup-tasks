/// <summary>
/// Manages the execution of startup tasks.
/// </summary>
public interface IStartupTaskEngine
{
    /// <summary>
    /// Runs all registered startup tasks.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Awaitable task.</returns>
    Task RunAllAsync(CancellationToken cancellationToken);
}
