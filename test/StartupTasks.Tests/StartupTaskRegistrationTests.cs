using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public sealed class StartupTaskRegistrationTests
{
    [Fact]
    public async Task ShouldCreateActionRegistration()
    {
        // Arrange
        var hasRan = false;
        Func<CancellationToken, Task> action = (cancellationToken) =>
        {
            hasRan = true;
            return Task.CompletedTask;
        };

        var isParallel = true;

        // Act
        var registration = new StartupTaskActionRegistration(action, isParallel);
        var task = registration.CreateTask(Mock.Of<IServiceProvider>());
        await task(CancellationToken.None);

        // Assert
        registration.Action.Should().BeSameAs(action, "because we expect the action to be the same");
        registration.RunInParallel.Should().Be(isParallel, "because we expect the parallel flag to be the same");
        task.Should().BeSameAs(action, "because we expect the task to be the same as the action");
        hasRan.Should().BeTrue("because we expect the action to have ran");
    }

    [Fact]
    public void ShouldThrowIfActionIsNull()
    {
        // Arrange
        Func<CancellationToken, Task> action = null;
        var isParallel = true;

        // Act
        Action action1 = () => new StartupTaskActionRegistration(action, isParallel);

        // Assert
        action1.Should().Throw<ArgumentNullException>("because we expected a null action to throw");
    }

    [Fact]
    public async Task ShouldCreateTypedRegistration()
    {
        // Arrange
        var services = new ServiceCollection();
        var startupTask = new EmptyStartupTask();
        services.AddSingleton(startupTask);
        var serviceProvider = services.BuildServiceProvider();
        var isParallel = true;

        // Act
        var registration = new StartupTaskTypedRegistration(startupTask.GetType(), isParallel);
        var task = registration.CreateTask(serviceProvider);
        await task(CancellationToken.None);


        // Assert
        registration.Type.Should().Be(startupTask.GetType(), "because we expect the type to be the same");
        registration.RunInParallel.Should().Be(isParallel, "because we expect the isParallel to be the same");
        startupTask.HasRan.Should().BeTrue("because we expect the startup task to have ran");
    }

    [Fact]
    public void ShouldThrowIfTypeIsNull()
    {
        // Arrange
        var type = (Type)null;
        var isParallel = true;

        // Act
        Action action = () => new StartupTaskTypedRegistration(type, isParallel);

        // Assert
        action.Should().Throw<ArgumentNullException>("because we expected a null type to throw");
    }

    [Fact]
    public async Task ShouldThrowIfServiceProviderCannotResolveType()
    {
        // Arrange
        var registration = new StartupTaskTypedRegistration(typeof(EmptyStartupTask), false);
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        // Act
        Func<Task> action = () => registration.CreateTask(serviceProvider)(CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<StartupTaskException>("because we expected a type that cannot be resolved to throw");
    }
}
