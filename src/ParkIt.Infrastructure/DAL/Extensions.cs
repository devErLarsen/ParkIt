using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkIt.Core.Repositories;
using ParkIt.Infrastructure.DAL.Repositories;

namespace ParkIt.Infrastructure.DAL;

internal static class Extensions
{
    public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetRequiredSection("database"));
        var dbOptions = new DatabaseOptions();
        var section = configuration.GetRequiredSection("database");
        section.Bind(dbOptions);
        services.AddDbContext<ParkItDbContext>(x => x.UseSqlServer(dbOptions.ConnectionString));

        services.AddScoped<IParkingSpotRepository, ParkingSpotRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        // background service for clearing out of date reservation.
        // services.AddHostedService<DbCleaningBackgroundService>();

        return services;
    }
}