using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkIt.Core.Entities;

namespace ParkIt.Infrastructure.DAL.Configurations;

internal sealed class CarSpotConfiguration : IEntityTypeConfiguration<CarSpot>
{
    public void Configure(EntityTypeBuilder<CarSpot> builder)
    {
    }
}