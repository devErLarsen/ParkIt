using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkIt.Core.Entities;

namespace ParkIt.Infrastructure.DAL.Configurations;

internal sealed class HandicapSpotConfiguration : IEntityTypeConfiguration<HandicapSpot>
{
    public void Configure(EntityTypeBuilder<HandicapSpot> builder)
    {
    }
}