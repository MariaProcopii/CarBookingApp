using CarBookingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingApp.Infrastructure.Configurations;

public class RideReviewConfiguration : IEntityTypeConfiguration<RideReview>
{
    public void Configure(EntityTypeBuilder<RideReview> builder)
    {
        builder.HasOne(r => r.Reviewer)
            .WithOne()
            .HasForeignKey<RideReview>("RideReviewerId")
            .IsRequired();

        builder.HasOne(r => r.Reviewee)
            .WithOne()
            .HasForeignKey<RideReview>("RideRevieweeId")
            .IsRequired();
        
        builder
            .Property<DateTime>("CreatedAt")
            .HasColumnType("datetime")
            .HasDefaultValueSql("now()")
            .IsRequired();
        
        builder.ToTable("RideReviews",
            t =>
            {
                t.HasCheckConstraint("CK_RideReviews_Rating", " \"Rating\" >= 0");
            });
        
        builder
            .Property<DateTime>("CreatedAt")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();
    }
}