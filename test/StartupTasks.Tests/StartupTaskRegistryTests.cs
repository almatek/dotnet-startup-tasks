using FluentAssertions;
using System;
using Xunit;

public sealed class StartupTaskRegistryTests
{
    [Fact]
    public void ShouldAddRegistration()
    {
        // Arrange
        var registry = new StartupTaskRegistry();
        var type = typeof(EmptyStartupTask);
        var isParallel = true;
        var registration = new StartupTaskTypedRegistration<EmptyStartupTask>(isParallel);

        // Act
        registry.AddRegistration(registration);

        // Assert
        registry.GetAll().Should().Contain(registration);
        registry.GetAll().Should()
            .ContainSingle("because we added one registration")
            .And.Subject.Should().Contain(registration);
    }

    [Fact]
    public void ShouldThrowIfRegistrationIsNull()
    {
        // Arrange
        var registry = new StartupTaskRegistry();
        var registration = (IStartupTaskRegistration)null;

        // Act
        Action action = () => registry.AddRegistration(registration);

        // Assert
        action.Should().Throw<ArgumentNullException>("because we expected a null registration to throw");
    }

    [Fact]
    public void ShouldNotAddDuplicateRegistration()
    {
        // Arrange
        var registry = new StartupTaskRegistry();
        
        // Act
        registry.AddRegistration(new StartupTaskTypedRegistration<EmptyStartupTask>(true));
        registry.AddRegistration(new StartupTaskTypedRegistration<EmptyStartupTask>(true));

        // Assert
        registry.GetAll().Should().ContainSingle("because we added a duplicate registration");
    }
}
