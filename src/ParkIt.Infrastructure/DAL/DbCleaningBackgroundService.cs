using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ParkIt.Infrastructure.DAL;

public class DbCleaningBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DbCleaningBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var today = DateTime.Today.AddDays(1);
        await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ParkItDbContext>();
            var parkingSpots = await context.ParkingSpots.ToListAsync(stoppingToken);
            var outDatedReservations = parkingSpots.SelectMany(x => x.Reservations)
                .Where(x => x.ReservationPeriod.Start < today).ToList();
            // var outDatedReservations = await context
            //     .Reservations
            //     .Where(x => x.ReservationPeriod.Start < today)
            //     .ToListAsync(cancellationToken: stoppingToken);
            if (outDatedReservations.Any())
            {
                context.RemoveRange(outDatedReservations);
                await context.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}