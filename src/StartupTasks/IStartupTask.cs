/// <summary>
/// The startup task interface.
/// </summary>
public interface IStartupTask
{
    /// <summary>
    /// Runs the startup task.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Awaitable task.</returns>
    Task RunAsync(CancellationToken cancellationToken);
}
