
using Microsoft.Extensions.DependencyInjection;
/// <summary>
/// Used to track a startup task registration using a type.
/// </summary>
public class StartupTaskTypedRegistration : IStartupTaskRegistration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskTypedRegistration"/> class.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="runInParallel"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartupTaskTypedRegistration(Type type, bool runInParallel)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        RunInParallel = runInParallel;
    }

    /// <summary>
    /// Gets or sets the startup task type.
    /// </summary>
    public Type Type { get; }

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
    /// <exception cref="StartupTaskException"></exception>
    public Func<CancellationToken, Task> CreateTask(IServiceProvider provider)
    {
        try
        {
            var task = (IStartupTask)provider.GetRequiredService(Type);
            return task.RunAsync;
        }
        catch (Exception ex)
        {
            throw new StartupTaskException($"Unable to create startup task of type '{Type.FullName}'.", ex);
        }
    }
}
