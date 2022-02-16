using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public sealed class StartupTaskEngineTests
{
    [Fact]
    public void ShouldThrowIfConstructorArgumentIsNull()
    {
        // Arrange
        // Act
        Action action1 = () => new StartupTaskEngine(null, Mock.Of<IStartupTaskRegistry>(), null);
        Action action2 = () => new StartupTaskEngine(Mock.Of<IServiceProvider>(), null, null);

        // Assert
        action1.Should().Throw<ArgumentNullException>("because we expected a null IServiceProvider to throw");
        action2.Should().Throw<ArgumentNullException>("because we expected a null IStartupTaskRegistry to throw");
    }

    [Fact]
    public async Task ShouldRunAllTasks()
    {
        // Arrange
        var services = new ServiceCollection();
        var startupTask1 = new EmptyStartupTask();
        var startupTask2 = new AnotherEmptyStartupTask();
        
        services.AddSingleton(startupTask1);
        services.AddSingleton(startupTask2);

        var registry = new StartupTaskRegistry();
        registry.AddRegistration(new StartupTaskTypedRegistration<EmptyStartupTask>(false));
        registry.AddRegistration(new StartupTaskTypedRegistration<AnotherEmptyStartupTask>(true));

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var engine = new StartupTaskEngine(serviceProvider, registry, null);
        await engine.RunAllAsync(CancellationToken.None);

        // Assert
        startupTask1.HasRan.Should().BeTrue("because we expect the first startup task to have run");
        startupTask2.HasRan.Should().BeTrue("because we expect the second startup task to have run");
    }
}
