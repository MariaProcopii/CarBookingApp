using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Application.Vehicles.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Vehicles.Queries;

public record GetAllUniqueVendorQuery : IRequest<List<String>>;

public class GetAllUniqueVendorQueryHandler : IRequestHandler<GetAllUniqueVendorQuery, List<String>>
{
    private readonly IVehicleRepository _repository;
    private readonly IMapper _mapper;

    public GetAllUniqueVendorQueryHandler(IVehicleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<String>> Handle(GetAllUniqueVendorQuery request, CancellationToken cancellationToken)
    {
        return  await _repository.GetUniqueVendorListAsynk();
        // return _mapper.Map<List<Vehicle>, List<VehicleVendorDTO>>(vehicles);
    }
}
