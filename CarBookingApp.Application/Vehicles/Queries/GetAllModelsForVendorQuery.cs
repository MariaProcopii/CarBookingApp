using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Vehicles.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Vehicles.Queries;

public class GetAllModelsForVendorQuery() : IRequest<List<String>>
{
    public string Vendor { get; set; }
}

public class GetAllModelsForVendorQueryHandler : IRequestHandler<GetAllModelsForVendorQuery, List<String>>
{
    private readonly IVehicleRepository _repository;
    private readonly IMapper _mapper;

    public GetAllModelsForVendorQueryHandler(IVehicleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<String>> Handle(GetAllModelsForVendorQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetModelsForVendorListAsynk(request.Vendor);
    }
}