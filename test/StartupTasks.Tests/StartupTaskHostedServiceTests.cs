using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public sealed class StartupTaskHostedServiceTests
{
    [Fact]
    public async Task ShouldRunAllTasks()
    {
        // Arrange
        var startupTask1 = new EmptyStartupTask();
        var startupTask2 = new AnotherEmptyStartupTask();
        var services = new ServiceCollection();
        var hasRan = false;
        services.AddLogging();
        services.AddStartupTasks()
            .Add(startupTask1, runInParallel: false)
            .Add(startupTask2, runInParallel: true)
            .AddAction((sp) =>
            {
                hasRan = true;
                return Task.CompletedTask;
            });

        var serviceProvider = services.BuildServiceProvider();
        var startupTaskHostedService = serviceProvider.GetRequiredService<IHostedService>();

        // Act
        await startupTaskHostedService.StartAsync(CancellationToken.None);

        // Assert
        startupTask1.HasRan.Should().BeTrue("because we expect the first startup task to have run");
        startupTask2.HasRan.Should().BeTrue("because we expect the second startup task to have run");
        hasRan.Should().BeTrue("because we expect the action to have run");
    }
}
