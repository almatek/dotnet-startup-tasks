using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

/// <summary>
/// Manages the execution of startup tasks.
/// </summary>
public class StartupTaskEngine : IStartupTaskEngine
{
    private readonly IServiceProvider _provider;
    private readonly IStartupTaskRegistry _registry;
    private readonly ILogger<StartupTaskEngine>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskEngine"/> class.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="registry"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartupTaskEngine(
        IServiceProvider provider,
        IStartupTaskRegistry registry,
        ILogger<StartupTaskEngine>? logger)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _logger = logger ?? NullLogger<StartupTaskEngine>.Instance;
    }

    /// <summary>
    /// Runs all registered startup tasks.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Awaitable task.</returns>
    public async Task RunAllAsync(CancellationToken cancellationToken)
    {
        using var scope = _provider.CreateScope();

        var sequentialTasks = _registry.GetAll()
            .Where(x => !x.RunInParallel)
            .Select(r => r.CreateTask(_provider))
            .ToList();

        var parallelTasks = _registry.GetAll()
            .Where(x => x.RunInParallel)
            .Select(r => r.CreateTask(_provider)(cancellationToken))
            .ToList();

        foreach (var task in sequentialTasks)
            await task(cancellationToken);

        await Task.WhenAll(parallelTasks);
    }
}
