public class ParallelStartupTask : IStartupTask
{
    private readonly ILogger<ParallelStartupTask> _logger;

    public ParallelStartupTask(ILogger<ParallelStartupTask> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public Task RunAsync(CancellationToken cancellationToken)
    {
        var random = new Random();
        var delay = random.Next(100, 500);

        _logger.LogInformation($"ParallelStartupTask ran after {delay} ms delay.");

        return Task.Delay(delay);
    }
}
