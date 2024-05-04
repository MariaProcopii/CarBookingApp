using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarBookingApp.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.UseTptMappingStrategy();
        
        builder.Property(u => u.FirstName)
            .HasMaxLength(50);
        
        builder.Property(u => u.LastName)
            .HasMaxLength(50);

        builder.Property(u => u.Gender)
        .HasConversion(g => g.ToString(), 
            g => Enum.Parse<Gender>(g));
            
        builder.ToTable("Users", t =>
        {
            t.HasCheckConstraint("CK_User_Age_IsAdult", "EXTRACT(YEAR FROM CURRENT_DATE) - extract(YEAR FROM \"DateOfBirth\") >= 18");
        });
        
        builder
            .Property(u => u.Email)
            .HasMaxLength(50);
        
        builder
            .HasIndex(u => u.Email)
            .IsUnique();
        
        builder
            .Property(u => u.PhoneNumber)
            .HasMaxLength(50);
        
        builder
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        builder
            .HasMany(u => u.BookedRides)
            .WithMany(r => r.Passengers)
            .UsingEntity<UserRide>(
                l => l.HasOne<Ride>().WithMany().HasForeignKey(e => e.RideId),
                r => r.HasOne<User>().WithMany().HasForeignKey(e => e.PassengerId)
                );
        
        builder
            .Property<DateTime>("CreatedAt")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()")
            .IsRequired();
    }
}