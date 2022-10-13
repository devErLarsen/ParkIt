using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkIt.Infrastructure.DAL;

namespace ParkIt.Test.Integration;

public class ParkItTestFactory : WebApplicationFactory<Program>
{
    private static readonly string _connectionString = "Server=localhost,1433;Database=ParkIt-test;User=sa;Password=Password123!;";
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        // builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ParkItDbContext));
            if (context != null)
            {
                services.Remove(context);
                var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
                    || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToArray();
                foreach (var option in options)
                {
                    services.Remove(option);
                }
            }
            // services.RemoveAll(typeof(ParkItDbContext));

            services.AddDbContext<ParkItDbContext>(options => options.UseSqlServer(_connectionString));

            var sp = services
                .BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var parkItDb = scopedServices.GetRequiredService<ParkItDbContext>();

            // parkItDb.Database.EnsureDeleted();
            parkItDb.Database.EnsureCreated();
            // parkItDb.Database.Migrate()
        });

    }

    protected override void Dispose(bool disposing)
    {
        using var scope = Services.CreateScope();
        var sp = scope.ServiceProvider;
        var context = sp.GetRequiredService<ParkItDbContext>();
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}