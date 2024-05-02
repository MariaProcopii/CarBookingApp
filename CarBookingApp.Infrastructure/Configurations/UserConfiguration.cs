using CarBookingApp.Enum;
using CarBookingApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(50);
        
        builder.Property(u => u.LastName)
            .HasMaxLength(50);

        builder.Property(u => u.Gender)
        .HasConversion(g => g.ToString(), 
            g => Enum.Parse<Gender>(g));

        builder.ToTable("Users", t =>
        {
            t.HasCheckConstraint("CK_User_Age_Adult", "age >= 18");
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
            .HasMany(u => u.BookedRides)
            .WithMany(r => r.Passengers)
            .UsingEntity<PassengerRide>();

        builder
            .HasMany(u => u.Vehicles)
            .WithMany();
        
        builder
            .Property<DateTime>("CreatedAt")
            .HasColumnType("datetime")
            .HasDefaultValueSql("now()")
            .IsRequired();
    }
}