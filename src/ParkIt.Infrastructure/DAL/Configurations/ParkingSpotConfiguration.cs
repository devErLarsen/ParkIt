using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkIt.Core.Entities;
using ParkIt.Core.ValueObjects;

namespace ParkIt.Infrastructure.DAL.Configurations;

internal sealed class ParkingSpotConfiguration : IEntityTypeConfiguration<ParkingSpot>
{
    public void Configure(EntityTypeBuilder<ParkingSpot> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        // builder.HasMany(x => x.Reservations).WithOne(x => x.ParkingSpot).HasForeignKey("ParkingSpotId")
        //     .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.SpotCode)
            .IsRequired();
        builder.HasIndex(x => x.SpotCode)
            .IsUnique();

        builder.HasDiscriminator<string>("SpotType")
            .HasValue<CarSpot>(nameof(CarSpot))
            .HasValue<BikeSpot>(nameof(BikeSpot))
            .HasValue<HandicapSpot>(nameof(HandicapSpot));

        //reservation
        var reservationConfig = builder.OwnsMany(x => x.Reservations);
        
        reservationConfig.Property(x => x.Id).ValueGeneratedNever();
        reservationConfig.HasKey(x => x.Id);
        
        reservationConfig.Property(x => x.NumberPlate)
            .IsRequired()
            .HasConversion(x => x.Value, x => new NumberPlate(x));
        // reservationConfig.OwnsOne(x => x.Vehicle, v =>
        // {
        //     v.Property(x => x.NumberPlate).IsRequired().HasConversion(x => x.Value, x => new NumberPlate(x));
        //     // v.HasKey(x => x.NumberPlate);
        //     // v.HasIndex(x => x.NumberPlate).IsUnique();
        // });
        // reservationConfig.OwnsOne(x => x.NumberPlate);
        reservationConfig.OwnsOne(x => x.ReservationPeriod, navigationBuilder =>
        {
            navigationBuilder.Property(x => x.Start).IsRequired();
            navigationBuilder.Property(x => x.End).IsRequired();
        });
    }
}
