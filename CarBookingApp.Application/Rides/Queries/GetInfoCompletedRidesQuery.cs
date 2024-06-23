using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Queries;

public record GetInfoCompletedRidesQuery ( int UserId ): IRequest<List<UserDTO>>;

public class GetInfoCompletedRidesQueryHandler : IRequestHandler<GetInfoCompletedRidesQuery, List<UserDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetInfoCompletedRidesQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<UserDTO>> Handle(GetInfoCompletedRidesQuery request, CancellationToken cancellationToken)
    {
        var completedRides =  await _repository.GetByPredicate<UserRide>(
            ur => ur.PassengerId == request.UserId 
                  && ur.RideStatus == RideStatus.COMPLETED 
                  && ur.ReviewDialog == ReviewDialog.NOTSENT,
            ur => ur.Ride,
            ur => ur.Ride.Owner,
            ur => ur.Ride.Owner.VehicleDetail,
            ur => ur.Ride.Owner.VehicleDetail.Vehicle
        );

        completedRides.ForEach(ur => ur.ReviewDialog = ReviewDialog.SENT);
        await _repository.Save();
        var owners = completedRides.Select(ur => ur.Ride.Owner).ToList();
        return _mapper.Map<List<Driver>, List<UserDTO>>(owners);
    }
}