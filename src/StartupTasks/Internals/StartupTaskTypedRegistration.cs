
using Microsoft.Extensions.DependencyInjection;
/// <summary>
/// Used to track a startup task registration using a type.
/// </summary>
public class StartupTaskTypedRegistration<T> : IStartupTaskRegistration where T : IStartupTask
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskTypedRegistration{T}"/> class.
    /// </summary>
    /// <param name="runInParallel"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartupTaskTypedRegistration(bool runInParallel)
    {
        RunInParallel = runInParallel;
    }

    /// <summary>
    /// Gets or sets the startup task type.
    /// </summary>
    public Type Type { get; } = typeof(T);

    /// <summary>
    /// Gets or sets whether the startup task should be executed in parallel.
    /// </summary>
    /// <remarks>
    /// If set to <c>true</c>, the startup tasks will be executed in parallel.
    /// If set to <c>false</c>, the startup tasks will be executed in the registered sequence.
    /// </remarks>
    public bool RunInParallel { get; }

    /// <summary>
    /// The key is the startup task type. This prevents duplicate registrations.
    /// </summary>
    public string Key => typeof(T).FullName ?? typeof(T).Name;

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
            var task = provider.GetRequiredService<T>();
            return task.RunAsync;
        }
        catch (Exception ex)
        {
            throw new StartupTaskException($"Unable to create startup task of type '{Type.FullName}'.", ex);
        }
    }

    /// <summary>
    /// Helper method to create a <see cref="StartupTaskActionRegistration"/>.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => Key.GetHashCode();

    /// <summary>
    /// Equals override.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals((IStartupTaskRegistration?)obj);

    /// <summary>
    /// Equality is based on the key.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(IStartupTaskRegistration? other) => other != null && Key == other.Key;
}
