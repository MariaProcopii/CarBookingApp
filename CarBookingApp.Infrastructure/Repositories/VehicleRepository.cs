using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using CarBookingApp.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CarBookingApp.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly CarBookingAppDbContext _carBookingAppDbContext;

    public VehicleRepository(CarBookingAppDbContext carBookingAppDbContext)
    {
        _carBookingAppDbContext = carBookingAppDbContext;
    }
    
    public async Task<List<String>> GetUniqueVendorListAsynk()
    {
        return await _carBookingAppDbContext
            .Vehicles
            .AsQueryable()
            .Select(v => v.Vender)
            .Distinct()
            .ToListAsync();
    }
    
    public async Task<List<String>> GetModelsForVendorListAsynk(string vendor)
    {
        return await _carBookingAppDbContext
            .Vehicles
            .AsQueryable()
            .Where(v => v.Vender == vendor)
            .Distinct()
            .Select(v => v.Model)
            .ToListAsync();
    }
}