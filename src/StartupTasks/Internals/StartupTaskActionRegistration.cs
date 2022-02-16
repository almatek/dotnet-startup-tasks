
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
    /// They key is unique for every action registered.
    /// </summary>
    public string Key => Guid.NewGuid().ToString();

    /// <summary>
    /// Creates a task out of this registration.
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public Func<CancellationToken, Task> CreateTask(IServiceProvider provider) => Action;

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
