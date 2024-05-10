using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Queries;
public record GetRideInfoByIdQuery(int RideId) : IRequest<RideFullInfoDTO>;

public class GetRideInfoByIdQueryHandler : IRequestHandler<GetRideInfoByIdQuery, RideFullInfoDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetRideInfoByIdQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RideFullInfoDTO> Handle(GetRideInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var ride = await _repository
            .GetByIdWithInclude<Ride>(request.RideId, 
                r => r.DestinationFrom, 
                r => r.DestinationTo,
                r => r.Owner,
                r => r.Owner.VehicleDetail,
                r => r.Owner.VehicleDetail.Vehicle,
                r => r.RideDetail,
                r => r.RideDetail.Facilities,
                r => r.Passengers);
        
        return _mapper.Map<Ride, RideFullInfoDTO>(ride);
    }
}