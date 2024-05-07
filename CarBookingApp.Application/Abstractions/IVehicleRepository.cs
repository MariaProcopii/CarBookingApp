using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Abstractions;

public interface IVehicleRepository
{
    public Task<List<String>> GetUniqueVendorListAsynk();
    public Task<List<String>> GetModelsForVendorListAsynk(string vendor);
}