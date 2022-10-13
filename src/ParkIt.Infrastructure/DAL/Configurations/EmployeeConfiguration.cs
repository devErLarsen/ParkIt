using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkIt.Core.Entities;
using ParkIt.Core.ValueObjects;

// using ParkIt.Core.Enums;

namespace ParkIt.Infrastructure.DAL.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.OwnsMany(x => x.Vehicles, v =>
        {
            v.Property(x => x.NumberPlate).IsRequired().HasConversion(x => x.Value, x => new NumberPlate(x));
            v.HasKey(x => x.NumberPlate);
            v.HasIndex(x => x.NumberPlate).IsUnique();
        });
    }
}
