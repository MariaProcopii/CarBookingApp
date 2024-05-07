using CarBookingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingApp.Infrastructure.Configurations;

public class VehicleDetailConfiguration : IEntityTypeConfiguration<VehicleDetail>
{
    public void Configure(EntityTypeBuilder<VehicleDetail> builder)
    {
        builder
            .HasIndex(vd => vd.RegistrationNumber)
            .IsUnique();

        builder
            .HasOne(vd => vd.Vehicle)
            .WithMany();

    }
}