using CarBookingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingApp.Infrastructure.Configurations;

public class RideDetailConfiguration : IEntityTypeConfiguration<RideDetail>
{
    public void Configure(EntityTypeBuilder<RideDetail> builder)
    {
        builder.Property(rd => rd.PickUpSpot)
            .HasMaxLength(50);
        
        builder.Property(rd => rd.Price)
            .HasDefaultValue(0);
        
        builder.ToTable("RideDetails",
            t =>
            {
                t.HasCheckConstraint("CK_RideDetail_Price_GreaterThanZero", "\"Price\" >= 0");
            });

        builder
            .HasMany(rd => rd.Facilities)
            .WithMany();

        builder
            .HasOne<Ride>()
            .WithOne(r => r.RideDetail)
            .HasForeignKey<RideDetail>()
            .IsRequired();
    }
}