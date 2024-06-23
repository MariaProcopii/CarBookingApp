using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Commands;

public record CompleteRideCommand(int RideId) : IRequest;

public class CompleteRideCommandHandler : IRequestHandler<CompleteRideCommand>
{
    private readonly IRepository _repository;

    public CompleteRideCommandHandler(IRepository repository)
    {
        _repository = repository;
    }


    public async Task Handle(CompleteRideCommand request, CancellationToken cancellationToken)
    {
        var completedUserRides = await _repository.GetByPredicate<UserRide>(ur => ur.RideId == request.RideId);
        
        if (completedUserRides.Count == 0)
        {
            throw new ActionNotAllowedException("ride: Can't complete the ride witch doesn't contain passenger.");
        }

        completedUserRides.ForEach(ur => ur.RideStatus = RideStatus.COMPLETED);
        await _repository.Save();
    }
}