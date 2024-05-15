using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using CarBookingApp.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CarBookingApp.IntegrationTests.Helpers;

public class DataContextBuilder : IDisposable
{
    private readonly CarBookingAppDbContext _dbContext;
    

    public DataContextBuilder(string dbName = "TestDatabase")
    {
        var options = new DbContextOptionsBuilder<CarBookingAppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new CarBookingAppDbContext(options);

        _dbContext = context;
    }
    
    public CarBookingAppDbContext GetContext()
    {
        _dbContext.Database.EnsureCreated();
        return _dbContext;
    }
    
    public void SeedDestinations(int number = 1)
    {
        var destinations = new List<Destination>();

        for (var i = 0; i < number; i++)
        {
            var id = i + 1;
            var name = $"Destination-{id}";

            var destination = new Destination
            {
                Id = id,
                Name = name,
                Region = Region.CENTRAL
            };

            destinations.Add(destination);
        }

        _dbContext.AddRange(destinations);
        _dbContext.SaveChanges();
    }
    
    public void SeedFacilities(int number = 1)
    {
        var destinations = new List<Facility>();

        for (var i = 0; i < number; i++)
        {
            var id = i + 1;
            var facilityType = $"FacilityType-{id}";

            var facility = new Facility
            {
                Id = id,
                FacilityType = facilityType
            };

            destinations.Add(facility);
        }

        _dbContext.AddRange(destinations);
        _dbContext.SaveChanges();
    }

    public void SeedVehicles(int number = 1)
    {
        var vehicles = new List<Vehicle>();
        
        for (var i = 0; i < number; i++)
        {
            var id = i + 1;
            var vendor = $"VendorName-{id}";
            var model = $"VendorModel-{id}";


            var vehicle = new Vehicle
            {
                Id = id,
                Vender = vendor,
                Model = model
            };

            vehicles.Add(vehicle);
        }

        _dbContext.AddRange(vehicles);
        _dbContext.SaveChanges();
    }
    
    public void SeedVehicleDetails(int number = 1)
    {
        SeedVehicles(number);

        var vehicles = _dbContext.Set<Vehicle>().ToList();
        var vehicleDetails = new List<VehicleDetail>();

        foreach (var vehicle in vehicles)
        {
            var vehicleDetail = new VehicleDetail
            {
                Id = number++,
                ManufactureYear = 2000 + vehicle.Id,
                RegistrationNumber = $"Reg-{vehicle.Id}",
                Vehicle = vehicle
            };

            vehicleDetails.Add(vehicleDetail);
        }

        _dbContext.AddRange(vehicleDetails);
        _dbContext.SaveChanges();
    }
    
    public void SeedUsers(int number = 1)
    {
        var users = new List<User>();

        for (var i = 0; i < number; i++)
        {
            var id = i + 1;
            var firstName = $"FirstName-{id}";
            var lastName = $"LastName-{id}";
            var email = $"user{id}@example.com";
            var phoneNumber = $"555-010{id}";

            var user = new User
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Gender = Gender.MALE,
                DateOfBirth = new DateTime(1990, 1, 1).AddDays(i),
                Email = email,
                PhoneNumber = phoneNumber,
                BookedRides = new List<Ride>()
            };

            users.Add(user);
        }

        _dbContext.AddRange(users);
        _dbContext.SaveChanges();
    }
    
    public void SeedDrivers(int number = 1)
    {
        var users = new List<User>();

        for (var i = 0; i < number; i++)
        {
            var id = i + 1;
            var firstName = $"FirstName-{id}";
            var lastName = $"LastName-{id}";
            var email = $"driver{id}@example.com";
            var phoneNumber = $"555-010{id}";

            var user = new Driver
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Gender = Gender.MALE,
                DateOfBirth = new DateTime(1990, 1, 1).AddDays(i),
                Email = email,
                PhoneNumber = phoneNumber,
                BookedRides = new List<Ride>(),
                YearsOfExperience = id
            };

            users.Add(user);
        }

        _dbContext.AddRange(users);
        _dbContext.SaveChanges();
    }
    
    public void SeedRides(int number = 1)
    {
        SeedDrivers(number);
        SeedDestinations(number);

        var drivers = _dbContext.Drivers.ToList();
        var destinations = _dbContext.Destinations.ToList();

        var rides = new List<Ride>();

        for (var i = 0; i < number; i++)
        {
            var ride = new Ride
            {
                DateOfTheRide = DateTime.Today.AddDays(i),
                DestinationFrom = destinations[i % destinations.Count],
                DestinationTo = destinations[(i + 1) % destinations.Count],
                TotalSeats = 4,
                Owner = drivers[i % drivers.Count],
                Passengers = new List<User>()
            };

            rides.Add(ride);
        }

        _dbContext.AddRange(rides);
        _dbContext.SaveChanges();
    }
    
    public void SeedUserRides(int number = 1)
    {
        var userRides = new List<UserRide>();

        for (var i = 0; i < number; i++)
        {
            var userRide = new UserRide
            {
                Id = i + 1,
                PassengerId = i + 1,
                RideId = i + 1,
                BookingStatus = BookingStatus.PENDING,
                RideStatus = RideStatus.UPCOMING
            };

            userRides.Add(userRide);
        }

        _dbContext.AddRange(userRides);
        _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}