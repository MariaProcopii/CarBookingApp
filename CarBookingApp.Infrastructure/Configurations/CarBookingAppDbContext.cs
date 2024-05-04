using System.Reflection;
using CarBookingApp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarBookingApp.Infrastructure.Configurations;

public class CarBookingAppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Driver> Drivers { get; set; } = default!;
    public DbSet<Destination> Destinations { get; set; } = default!;
    public DbSet<Entity> Entities { get; set; } = default!;
    public DbSet<Facility> Facilities { get; set; } = default!;
    public DbSet<Ride> Rides { get; set; } = default!;
    public DbSet<RideDetail> RideDetails { get; set; } = default!;
    public DbSet<RideReview> RideReviews { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;

    public CarBookingAppDbContext()
    {
    }

    public CarBookingAppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=carbookingapp;Username=maria;Password=maria;";
        optionsBuilder
            .UseNpgsql(connectionString)
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Ignore<Entity>();
    }
}