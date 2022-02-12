/// <summary>
/// Manages registration of startup tasks with the dependency injection container.
/// </summary>
public class StartupTaskRegistry : IStartupTaskRegistry
{
    private readonly List<IStartupTaskRegistration> _registrations = new List<IStartupTaskRegistration>();

    /// <summary>
    /// Gets all the registrations.
    /// </summary>
    public IEnumerable<IStartupTaskRegistration> GetAll() => _registrations;

    /// <summary>
    /// Adds a startup task registration.
    /// </summary>
    /// <param name="registration">The registration to add.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void AddRegistration(IStartupTaskRegistration registration)
    {
        if (registration is null)
            throw new ArgumentNullException(nameof(registration));

        _registrations.Add(registration);
    }
}
