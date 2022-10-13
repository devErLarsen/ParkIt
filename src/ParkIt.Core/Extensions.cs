using Microsoft.Extensions.DependencyInjection;
using ParkIt.Core.Services;

namespace ParkIt.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IReserveService, ReserveService>();
        return services;
    }
}