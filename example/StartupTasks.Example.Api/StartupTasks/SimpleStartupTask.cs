public class SimpleStartupTask : IStartupTask
{
    private readonly ILogger<SimpleStartupTask> _logger;

    public SimpleStartupTask(ILogger<SimpleStartupTask> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("SimpleStartupTask ran.");

        return Task.Delay(1000);
    }
}
