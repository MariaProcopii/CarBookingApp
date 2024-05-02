using CarBookingApp.Enum;
using CarBookingApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

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