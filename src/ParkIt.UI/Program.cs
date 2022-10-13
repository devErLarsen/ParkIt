using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using ParkIt.UI;
using ParkIt.UI.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
// builder.RootComponents.Add<ParkIt.UI.App>("#app");
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.NewestOnTop = false;
});

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddTransient<JwtMessageHandler>();
// builder.Services.AddScoped(provider => new JwtMessageHandler())

builder.Services.AddHttpClient("ParkItApi",
    // client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    client => client.BaseAddress = new Uri("http://localhost:5000"))
    .AddHttpMessageHandler<JwtMessageHandler>();

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("ParkItApi"));

await builder.Build().RunAsync();