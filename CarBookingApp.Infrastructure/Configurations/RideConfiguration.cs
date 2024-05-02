using CarBookingApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class RideConfiguration : IEntityTypeConfiguration<Ride>
{
    public void Configure(EntityTypeBuilder<Ride> builder)
    {
        builder
            .HasOne(r => r.DestinationFrom)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(r => r.DestinationFrom)
            .HasMaxLength(50);
        
        builder
            .HasOne(r => r.DestinationTo)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.Property(r => r.DestinationTo)
            .HasMaxLength(50);
        
        builder
            .Property(r => r.AvailableSeats)
            .HasColumnName("available_seats")
            .HasDefaultValue(1);

        builder.ToTable("Rides",
            t =>
            {
                t.HasCheckConstraint("CK_Ride_AvailableSeats", "available_seats >= 1 AND available_seats <= 6");
            });

        builder
            .HasOne(r => r.Owner)
            .WithMany(u => u.CreatedRides)
            .IsRequired();

        builder
            .HasOne(r => r.RideDetail)
            .WithOne(r => r.Ride)
            .HasForeignKey<RideDetail>();
        
        builder
            .Property<DateTime>("CreatedAt")
            .HasColumnType("datetime")
            .HasDefaultValueSql("now()")
            .IsRequired();
    }
}