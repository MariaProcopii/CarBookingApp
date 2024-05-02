using CarBookingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingApp.Infrastructure.Configurations;

public class RideConfiguration : IEntityTypeConfiguration<Ride>
{
    public void Configure(EntityTypeBuilder<Ride> builder)
    {
        builder
            .HasOne(r => r.DestinationFrom)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder
            .HasOne(r => r.DestinationTo)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
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
            .Property<DateTime>("CreatedAt")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();
    }
}