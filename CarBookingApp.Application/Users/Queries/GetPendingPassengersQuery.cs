using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Queries;
public record GetPendingPassengersQuery(int UserId) : IRequest<List<UserDTO>>;

public class GetPendingPassengersQueryHandler : IRequestHandler<GetPendingPassengersQuery, List<UserDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetPendingPassengersQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<UserDTO>> Handle(GetPendingPassengersQuery request, CancellationToken cancellationToken)
    {
        var userRides = await _repository
            .GetByPredicate<UserRide>(ur => ur.BookingStatus == BookingStatus.PENDING 
                                    && ur.Ride.Owner.Id == request.UserId, ur => ur.Passenger,
                                    ur => ur.Ride.Owner);

        var rides = userRides.Select(ur => ur.Passenger).ToList();
        
        return _mapper.Map<List<User>, List<UserDTO>>(rides);
    }
}