using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Enum;
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
        var approvedUserRides = await _repository.GetByPredicate<UserRide>(
            ur => ur.BookingStatus == BookingStatus.APPROVED && ur.Ride.Id == request.RideId);

        var approvedPassengersIdList = approvedUserRides.Select(ur => ur.PassengerId).ToList();
        
        
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
        
        var filteredPassengers = ride.Passengers.Where(u => approvedPassengersIdList.Contains(u.Id)).ToList();
        ride.Passengers = filteredPassengers;
        return _mapper.Map<Ride, RideFullInfoDTO>(ride);
    }
}