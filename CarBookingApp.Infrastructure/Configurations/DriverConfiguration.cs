using CarBookingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingApp.Infrastructure.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder
            .HasMany(u => u.Vehicles)
            .WithMany();
        
        builder.ToTable("Drivers",
            t =>
            {
                t.HasCheckConstraint("CK_Driver_Years_Of_EXP_PositiveNr", " \"YearsOfExperience\" >= 0");
            });
    }
}