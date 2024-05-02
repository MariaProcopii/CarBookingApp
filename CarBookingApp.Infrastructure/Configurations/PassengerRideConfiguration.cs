using CarBookingApp.Enum;
using CarBookingApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class PassengerRideConfiguration : IEntityTypeConfiguration<PassengerRide>
{
    public void Configure(EntityTypeBuilder<PassengerRide> builder)
    {
        builder.HasKey(pr => new { pr.PassengerId, pr.RideId });
        
        builder.Property(pr => pr.BookingStatus)
            .HasConversion(b => b.ToString(), 
                b => Enum.Parse<BookingStatus>(b))
            .HasDefaultValue(BookingStatus.PENDING);
        
        builder.Property(pr => pr.RideStatus)
            .HasConversion(r => r.ToString(), 
                r => Enum.Parse<RideStatus>(r))
            .HasDefaultValue(RideStatus.UPCOMING);
    }
}