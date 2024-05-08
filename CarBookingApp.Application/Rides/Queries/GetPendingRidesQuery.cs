using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Queries;

public record GetPendingRidesQuery(int UserId) : IRequest<List<RideDTO>>;

public class GetPendingRidesQueryHandler : IRequestHandler<GetPendingRidesQuery, List<RideDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetPendingRidesQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RideDTO>> Handle(GetPendingRidesQuery request, CancellationToken cancellationToken)
    {
        var userRides = await _repository
            .GetByPredicate<UserRide>(ur => ur.BookingStatus == BookingStatus.PENDING
                                            && ur.PassengerId == request.UserId, ur => ur.Ride,
                ur => ur.Ride.DestinationTo, ur => ur.Ride.DestinationFrom);

        var rides = userRides.Select(ur => ur.Ride).ToList();
        
        return _mapper.Map<List<Ride>, List<RideDTO>>(rides);
    }
}