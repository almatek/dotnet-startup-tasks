using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

// Setup logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Disable 'Server' in response header for security purposes
    builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

    // Configure logging
    builder.WebHost.UseSerilog();

    // Configure the database
    builder.Services.AddDbContext<ExampleDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    // Configure application
    var startupTasks = builder.Services.AddStartupTasks();

    // Run one simple task that will print a message to the console
    startupTasks.Add<SimpleStartupTask>();

    // Run 3 parallel tasks that will have random delays
    startupTasks.Add<ParallelStartupTask>(runInParallel: true);
    startupTasks.Add<ParallelStartupTask>(runInParallel: true);
    startupTasks.Add<ParallelStartupTask>(runInParallel: true);

    var app = builder.Build();
    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
