using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingApp.Infrastructure.Configurations;

public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
{
    public void Configure(EntityTypeBuilder<Destination> builder)
    {
        builder.Property(d => d.Name)
            .HasMaxLength(50);
        
        builder.Property(d => d.Region)
            .HasConversion(r => r.ToString(), 
                r => Enum.Parse<Region>(r));
    }
}