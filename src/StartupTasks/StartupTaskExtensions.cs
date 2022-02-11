using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency injection container extension methods.
/// </summary>
public static class StartupTaskExtensions
{
    /// <summary>
    /// Adds startup tasks to the dependency injection container.
    /// </summary>
    /// <param name="services">The dependency injection container.</param>
    /// <returns>IStartupTaskBuilder</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IStartupTaskBuilder AddStartupTasks(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        var registry = new StartupTaskRegistry();
        services.AddSingleton<IStartupTaskRegistry>(registry);
        services.AddHostedService<StartupTaskHostedService>();

        return new StartupTaskBuilder(registry, services);
    }
}
