
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Registers startup tasks with dependency injection container.
/// </summary>
public class StartupTaskBuilder : IStartupTaskBuilder
{
    private readonly IStartupTaskRegistry _registry;
    private readonly IServiceCollection _services;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskBuilder"/> class.
    /// </summary>
    /// <param name="registry">The startup task registry.</param>
    /// <param name="services">The dependency injection container.</param>
    public StartupTaskBuilder(IStartupTaskRegistry registry, IServiceCollection services)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Adds a startup task action.
    /// </summary>
    /// <param name="action">Action to run.</param>
    /// <param name="runInParallel">Whether the task runs in parallel or not.</param>
    /// <returns>IStartupTaskBuilder</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public IStartupTaskBuilder AddAction(Func<CancellationToken, Task> action, bool runInParallel = false)
    {
        if (action is null)
            throw new ArgumentNullException(nameof(action));

        _registry.AddRegistration(new StartupTaskActionRegistration(action, runInParallel));

        return this;
    }

    /// <summary>
    /// Adds a startup task.
    /// </summary>
    /// <typeparam name="T">Type of IStartupTask.</typeparam>
    /// <param name="runInParallel">Whether the task runs in parallel or not.</param>
    /// <returns>IStartupTaskBuilder</returns>
    public IStartupTaskBuilder Add<T>(bool runInParallel = false) where T : class, IStartupTask
    {
        _services.AddScoped<T>();
        _registry.AddRegistration(new StartupTaskTypedRegistration(typeof(T), runInParallel));

        return this;
    }

    /// <summary>
    /// Adds a startup task instance.
    /// </summary>
    /// <typeparam name="T">The type of IStartupTask.</typeparam>
    /// <param name="instance">The IStartupTask instance.</param>
    /// <param name="runInParallel">Whether the task runs in parallel or not.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public IStartupTaskBuilder Add<T>(T instance, bool runInParallel = false) where T : class, IStartupTask
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        _services.AddSingleton(instance);
        _registry.AddRegistration(new StartupTaskTypedRegistration(typeof(T), runInParallel));

        return this;
    }

    /// <summary>
    /// Adds a startup task with a custom resolver.
    /// </summary>
    /// <typeparam name="T">The type of IStartupTask.</typeparam>
    /// <param name="implementationFactory">The implementation factory.</param>
    /// <param name="runInParallel">Whether the task runs in parallel or not.</param>
    /// <returns>IStartupTaskBuilder</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public IStartupTaskBuilder Add<T>(Func<IServiceProvider, T> implementationFactory, bool runInParallel = false) where T : class, IStartupTask
    {
        if (implementationFactory is null)
            throw new ArgumentNullException(nameof(implementationFactory));

        _services.AddScoped<T>(implementationFactory);
        _registry.AddRegistration(new StartupTaskTypedRegistration(typeof(T), runInParallel));

        return this;
    }
}
