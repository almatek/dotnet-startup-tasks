/// <summary>
/// Exception thrown when something goes wrong during startup tasks.
/// </summary>
public class StartupTaskException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public StartupTaskException(string message) : base(message)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="StartupTaskException"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public StartupTaskException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
