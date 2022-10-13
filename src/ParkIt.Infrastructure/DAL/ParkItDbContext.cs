using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParkIt.Core.Entities;
using ParkIt.Infrastructure.Auth;

namespace ParkIt.Infrastructure.DAL;

public class ParkItDbContext : IdentityDbContext<ParkItUser>
{
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<Employee> Employees { get; set; }
    // public DbSet<Reservation> Reservations { get; set; }

    public ParkItDbContext(DbContextOptions<ParkItDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        // var userId = Guid.NewGuid();
        // modelBuilder.Entity<Employee>().HasData(new Employee(userId, Guid.NewGuid().ToString(), "Erik", "erik@test.com", DateTime.Now));
        // modelBuilder.Entity<ParkingSpot>().HasData(
        //     new("P1"),
        //     new("P2"),
        //     new("P3"),
        //     new("P4"),
        //     new("P5")
        // );
        // modelBuilder.Entity<Reservation>().HasData(new Reservation(Guid.NewGuid(), userId, "Erik",
        //     new Core.ValueObjects.NumberPlate("xyz123"), new Core.ValueObjects.ReservationPeriod(DateTime.Now, DateTime.Now.AddHours(4))));
    }
}