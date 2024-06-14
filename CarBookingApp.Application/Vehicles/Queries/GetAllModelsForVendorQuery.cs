using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Vehicles.Queries;
public record GetAllModelsForVendorQuery(string Vendor) : IRequest<List<String>>;

public class GetAllModelsForVendorQueryHandler : IRequestHandler<GetAllModelsForVendorQuery, List<String>>
{
    private readonly IRepository _repository;

    public GetAllModelsForVendorQueryHandler(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<List<String>> Handle(GetAllModelsForVendorQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _repository.GetByPredicate<Vehicle>(v => v.Vender == request.Vendor);
        return vehicles.Select(v => v.Model).Distinct().ToList();
    }
}