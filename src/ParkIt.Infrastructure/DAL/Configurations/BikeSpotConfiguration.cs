using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkIt.Core.Entities;

namespace ParkIt.Infrastructure.DAL.Configurations;

internal sealed class BikeSpotConfiguration : IEntityTypeConfiguration<BikeSpot>
{
    public void Configure(EntityTypeBuilder<BikeSpot> builder)
    {
        builder.Property(x => x.SpotCapacity).IsRequired();
    }
}