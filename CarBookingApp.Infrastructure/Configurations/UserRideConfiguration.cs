using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingApp.Infrastructure.Configurations;

public class UserRideConfiguration : IEntityTypeConfiguration<UserRide>
{
    public void Configure(EntityTypeBuilder<UserRide> builder)
    {
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