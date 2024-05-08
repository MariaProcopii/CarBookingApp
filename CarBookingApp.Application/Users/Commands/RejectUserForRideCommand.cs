using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Commands;
public class RejectUserForRideCommand : IRequest<int>
{
    public int RideId { get; set; }
    public int PassengerId { get; set; }
}

public class RejectUserForRideCommandHandler : IRequestHandler<RejectUserForRideCommand, int>
{
    private readonly IRepository _repository;

    public RejectUserForRideCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(RejectUserForRideCommand request, CancellationToken cancellationToken)
    {
        var userRide = await _repository.GetByPredicate<UserRide>(ur => ur.RideId == request.RideId
                                                                        && ur.PassengerId == request.PassengerId);
        userRide.First().BookingStatus = BookingStatus.REJECTED;
        await _repository.Save();
        return request.RideId;
    }
}