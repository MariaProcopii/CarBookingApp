using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
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
        if (!userRide.Any())
        {
            throw new ActionNotAllowedException($"No user with id {request.PassengerId} is waiting approval " +
                                                $"for ride with id {request.RideId}");
        }

        if (userRide.First().BookingStatus == BookingStatus.APPROVED)
        {
            throw new ActionNotAllowedException("You cannot reject already approved user");
        }
        
        userRide.First().BookingStatus = BookingStatus.REJECTED;
        await _repository.Save();
        return request.RideId;
    }
}