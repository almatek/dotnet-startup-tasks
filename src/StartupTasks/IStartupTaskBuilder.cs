/// <summary>
/// Registers startup tasks with dependency injection container.
/// </summary>
public interface IStartupTaskBuilder
{
    /// <summary>
    /// Adds a startup task action.
    /// </summary>
    /// <param name="action">Action to run.</param>
    /// <param name="runInParallel">Whether the task runs in parallel or not.</param>
    /// <returns>IStartupTaskBuilder</returns>
    IStartupTaskBuilder AddAction(Func<CancellationToken, Task> action, bool runInParallel = false);

    /// <summary>
    /// Adds a startup task.
    /// </summary>
    /// <typeparam name="T">Type of IStartupTask.</typeparam>
    /// <param name="runInParallel">Whether the task runs in parallel or not.</param>
    /// <returns>IStartupTaskBuilder</returns>
    IStartupTaskBuilder Add<T>(bool runInParallel = false) where T : class, IStartupTask;

    /// <summary>
    /// Adds a startup task instance.
    /// </summary>
    /// <typeparam name="T">The type of IStartupTask.</typeparam>
    /// <param name="instance">The IStartupTask instance.</param>
    /// <param name="runInParallel">Whether the task runs in parallel or not.</param>
    /// <returns>IStartupTaskBuilder</returns>
    IStartupTaskBuilder Add<T>(T instance, bool runInParallel = false) where T : class, IStartupTask;

    /// <summary>
    /// Adds a startup task with a custom resolver.
    /// </summary>
    /// <typeparam name="T">The type of IStartupTask.</typeparam>
    /// <param name="implementationFactory">The implementation factory.</param>
    /// <param name="runInParallel">Whether the task runs in parallel or not.</param>
    /// <returns>IStartupTaskBuilder</returns>
    IStartupTaskBuilder Add<T>(Func<IServiceProvider, T> implementationFactory, bool runInParallel = false) where T : class, IStartupTask;
}
