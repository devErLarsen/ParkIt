using ParkIt.Api.Middleware;
using ParkIt.Core;
using ParkIt.Application;
using ParkIt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddSingleton<ErrorHandler>()
    .AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<ErrorHandler>();
app.UseInfrastructure();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }