using CarBookingApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class RideReviewConfiguration : IEntityTypeConfiguration<RideReview>
{
    public void Configure(EntityTypeBuilder<RideReview> builder)
    {
        builder.Property(r => r.Ride)
            .IsRequired();
        
        builder.Property(r => r.Passenger)
            .IsRequired();
        
        builder
            .Property<DateTime>("CreatedAt")
            .HasColumnType("datetime")
            .HasDefaultValueSql("now()")
            .IsRequired();
        
        builder.ToTable("RideReviews",
            t =>
            {
                t.HasCheckConstraint("CK_RideReviews_Rating", "rating >= 0");
            });
    }
}