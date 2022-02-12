using System.Threading;
using System.Threading.Tasks;

internal sealed class AnotherEmptyStartupTask : IStartupTask
{
    public bool HasRan { get; set; }

    public Task RunAsync(CancellationToken cancellationToken)
    {
        HasRan = true;

        return Task.CompletedTask;
    }
}
