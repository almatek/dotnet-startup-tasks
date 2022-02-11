using Microsoft.Extensions.Hosting;

/// <summary>
/// Hosted service for executing startup tasks.
/// </summary>
public class StartupTaskHostedService : IHostedService
{
    private readonly IStartupTaskEngine _engine;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskHostedService"/> class.
    /// </summary>
    /// <param name="engine"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartupTaskHostedService(IStartupTaskEngine engine)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
    }

    /// <summary>
    /// Starts the startup tasks.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _engine.RunAllAsync(cancellationToken);
    }

    /// <summary>
    /// Stops the startup tasks.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
