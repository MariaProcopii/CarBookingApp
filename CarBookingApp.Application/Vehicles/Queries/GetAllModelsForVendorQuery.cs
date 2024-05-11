using CarBookingApp.Application.Abstractions;
using MediatR;

namespace CarBookingApp.Application.Vehicles.Queries;

public class GetAllModelsForVendorQuery() : IRequest<List<String>>
{
    public string Vendor { get; set; }
}

public class GetAllModelsForVendorQueryHandler : IRequestHandler<GetAllModelsForVendorQuery, List<String>>
{
    private readonly IVehicleRepository _repository;

    public GetAllModelsForVendorQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<String>> Handle(GetAllModelsForVendorQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetModelsForVendorListAsynk(request.Vendor);
    }
}