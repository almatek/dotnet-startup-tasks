/// <summary>
/// Used to track a startup task registration using a type.
/// </summary>
public class StartupTaskTypedRegistration : IStartupTaskRegistration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskTypedRegistration"/> class.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isParallel"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartupTaskTypedRegistration(Type type, bool isParallel)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        IsParallel = isParallel;
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
    public bool IsParallel { get; }
}
