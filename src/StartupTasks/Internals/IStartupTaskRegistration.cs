/// <summary>
/// Used to track a startup task registration.
/// </summary>
public interface IStartupTaskRegistration : IEquatable<IStartupTaskRegistration>
{
    /// <summary>
    /// Gets a unique value that can be used to identify this registration.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Gets whether the startup task should be executed in parallel.
    /// </summary>
    bool RunInParallel { get; }

    /// <summary>
    /// Creates a task out of this registration
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    Func<CancellationToken, Task> CreateTask(IServiceProvider provider);
}
