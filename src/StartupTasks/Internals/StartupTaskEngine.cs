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
    /// <exception cref="NotSupportedException"></exception>
    public async Task RunAllAsync(CancellationToken cancellationToken)
    {
        using var scope = _provider.CreateScope();
        var tasksFuncs = new List<(Func<CancellationToken, Task>, bool)>();

        foreach (var registration in _registry.Registrations)
        {
            if (registration is StartupTaskTypedRegistration typedReg)
            {
                var task = (IStartupTask)scope.ServiceProvider.GetRequiredService(typedReg.Type);
                tasksFuncs.Add((task.RunAsync, typedReg.IsParallel));

            }
            else if (registration is StartupTaskActionRegistration actionReg)
            {
                tasksFuncs.Add((actionReg.Action, actionReg.IsParallel));
            }
            else
            {
                throw new NotSupportedException($"Unsupported startup task registration type: {registration.GetType().FullName}");
            }
        }

        var parallelTasks = tasksFuncs.Where(x => x.Item2).Select(x => x.Item1);
        var sequentialTasks = tasksFuncs.Where(x => !x.Item2).Select(x => x.Item1);
        var runningTasks = new List<Task>();

        foreach (var task in parallelTasks)
            runningTasks.Add(task(cancellationToken));

        foreach (var task in sequentialTasks)
            await task(cancellationToken);

        await Task.WhenAll(runningTasks);
    }
}
