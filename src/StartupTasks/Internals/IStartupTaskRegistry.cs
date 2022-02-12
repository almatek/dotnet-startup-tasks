/// <summary>
/// Manages registration of startup tasks with the dependency injection container.
/// </summary>
public interface IStartupTaskRegistry
{
    /// <summary>
    /// Gets all the registrations.
    /// </summary>
    IEnumerable<IStartupTaskRegistration> GetAll();

    /// <summary>
    /// Adds a startup task registration.
    /// </summary>
    /// <param name="registration">The registration to add.</param>
    void AddRegistration(IStartupTaskRegistration registration);
}
