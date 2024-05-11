using CarBookingApp.Application.Abstractions;
using MediatR;

namespace CarBookingApp.Application.Vehicles.Queries;

public record GetAllUniqueVendorQuery : IRequest<List<String>>;

public class GetAllUniqueVendorQueryHandler : IRequestHandler<GetAllUniqueVendorQuery, List<String>>
{
    private readonly IVehicleRepository _repository;

    public GetAllUniqueVendorQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<String>> Handle(GetAllUniqueVendorQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetUniqueVendorListAsynk();
    }
}
