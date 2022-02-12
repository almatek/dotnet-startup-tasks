
/// <summary>
/// Used to track a startup task registration using an action delegate.
/// </summary>
public class StartupTaskActionRegistration : IStartupTaskRegistration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskActionRegistration"/> class.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="runInParallel"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartupTaskActionRegistration(Func<CancellationToken, Task> action, bool runInParallel)
    {
        Action = action ?? throw new ArgumentNullException(nameof(action));
        RunInParallel = runInParallel;
    }

    /// <summary>   
    /// Gets or sets the startup task action.
    /// </summary>
    public Func<CancellationToken, Task> Action { get; }

    /// <summary>
    /// Gets or sets whether the startup task should be executed in parallel.
    /// </summary>
    /// <remarks>
    /// If set to <c>true</c>, the startup tasks will be executed in parallel.
    /// If set to <c>false</c>, the startup tasks will be executed in the registered sequence.
    /// </remarks>
    public bool RunInParallel { get; }

    /// <summary>
    /// Creates a task out of this registration.
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public Func<CancellationToken, Task> CreateTask(IServiceProvider provider) => Action;
}
