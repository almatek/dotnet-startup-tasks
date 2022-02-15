# StartupTasks
[![Build and test](https://github.com/almatek/dotnet-startup-tasks/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/almatek/dotnet-startup-tasks/actions/workflows/build-and-test.yml)
## About The Project
This is a simple library written in C# .NET 6 for adding startup tasks using hosted services.

## Getting Started
Add the nuget package reference to your project

* Using dotnet-cli `dotnet add package StartupTasks`
* Using Package Manager `Install-Package StartupTasks -Version`

Create as class that implements `IStartupTask`
```csharp
public class MyStartupTask : IStartupTask
{
    public Task RunAsync(CancellationToken cancellationToken)
    {
        // Startup logic goes here
        return Task.CompletedTask;
    }
}
```

Register startup tasks with `IServiceCollection` and add your startup task. Code block below shows common usage scenarios.
```csharp
using StartupTasks;

var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddStartupTasks()

    // Adds a startup task that runs first.
    .Add<MyStartupTask>(); 

    // Adds another startup task that runs second.
    .Add<AnotherStartupTask>();

    // Adds a startup task that runs in parlallel
    // and begins before the sequential tasks are
    // executed.
    .Add<ParallelStartupTask>(runInParllel: true)

    // Adds a startup task without a class file.
    // Delegate takes a CancellationToken as parameter
    // and returns a Task.
    .AddAction(async (cancellationToken) => 
        await Console.out.WriteLineAsync("Let's begin", cancellationToken),
        runInParallel: true)

var app = builder.Build();
app.Run();
```


<!-- ROADMAP -->
## Roadmap

- [x] Add startup tasks executed in hosted services
- [ ] Add startup task health check report after all have ran
- [ ] Add option to verification of registered tasks can be resolved at before execution 

See the [open issues](https://github.com/almatek/dotnet-startup-tasks/issues) for a full list of proposed features (and known issues).

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

Albin Ma - ama@almatek.io

Project Link: [https://github.com/almatek/dotnet-startup-tasks](https://github.com/almatek/dotnet-startup-tasks)