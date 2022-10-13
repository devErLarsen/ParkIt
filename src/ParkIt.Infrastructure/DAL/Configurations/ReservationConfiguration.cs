// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using ParkIt.Core.Entities;
// using ParkIt.Core.ValueObjects;
//
// namespace ParkIt.Infrastructure.DAL.Configurations;
//
// internal sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
// {
//     public void Configure(EntityTypeBuilder<Reservation> builder)
//     {
//         builder.Property(x => x.Id).ValueGeneratedNever();
//         builder.HasKey(x => x.Id);
//         builder.Property(x => x.NumberPlate)
//             .IsRequired()
//             .HasConversion(x => x.Value, x => new NumberPlate(x));
//         builder.OwnsOne(x => x.ReservationPeriod, navigationBuilder =>
//         {
//             navigationBuilder.Property(x => x.Start).IsRequired();
//             navigationBuilder.Property(x => x.End).IsRequired();
//         });
//     }
// }
