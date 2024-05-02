using CarBookingApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class RideDetailConfiguration : IEntityTypeConfiguration<RideDetail>
{
    public void Configure(EntityTypeBuilder<RideDetail> builder)
    {
        builder.Property(rd => rd.PickUpSpot)
            .HasMaxLength(50);
        
        builder.ToTable("RideDetails",
            t =>
            {
                t.HasCheckConstraint("CK_RideDetail_Price_GreaterThanZero", "price >= 0");
            });
        
        builder.Property(rd => rd.Price)
            .HasDefaultValue(0);

        builder
            .HasMany(rd => rd.Facilities)
            .WithMany();
    }
}