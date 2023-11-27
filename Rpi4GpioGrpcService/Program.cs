using RaspberryPi4GpioServiceLibrary;
using Rpi4GpioGrpcService.Services;
using Serilog;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();

try
{
    Log.Information("Starting web application.");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddGrpc();
    builder.Services.AddSingleton<GpioService>();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.MapGrpcService<CommanderService>();
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
