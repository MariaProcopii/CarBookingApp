using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Queries;

public record GetBookedRidesQuery(int UserId) : IRequest<List<RideDTO>>;

public class GetBookedRidesQueryHandler : IRequestHandler<GetBookedRidesQuery, List<RideDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetBookedRidesQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RideDTO>> Handle(GetBookedRidesQuery request, CancellationToken cancellationToken)
    {
        var userRides = await _repository
            .GetByPredicate<UserRide>(ur => ur.BookingStatus == BookingStatus.APPROVED
                && ur.PassengerId == request.UserId, ur => ur.Ride,
                ur => ur.Ride.DestinationTo, ur => ur.Ride.DestinationFrom);

        var rides = userRides.Select(ur => ur.Ride).ToList();
        
        return _mapper.Map<List<Ride>, List<RideDTO>>(rides);
    }
}