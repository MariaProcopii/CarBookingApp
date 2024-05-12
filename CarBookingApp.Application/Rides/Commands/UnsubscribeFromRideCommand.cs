using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Commands;

public class UnsubscribeFromRideCommand() : IRequest<RideShortInfoDTO>
{
    public int RideId { get; set; }
    public int PassengerId { get; set; }
}

public class UnsubscribeFromRideCommandHandler : IRequestHandler<UnsubscribeFromRideCommand, RideShortInfoDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public UnsubscribeFromRideCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RideShortInfoDTO> Handle(UnsubscribeFromRideCommand request, CancellationToken cancellationToken)
    {
        var userRide = await _repository.GetByPredicate<UserRide>(ur => ur.RideId == request.RideId
                                                                        && ur.PassengerId == request.PassengerId);

        if (userRide.First().RideStatus == RideStatus.ONGOING)
        {
            throw new ActionNotAllowedException("You cannot cancel an ongoing ride");
        }
        
        var ride = await _repository.GetByIdWithInclude<Ride>(request.RideId, 
            r => r.DestinationFrom, r => r.DestinationTo, r => r.Owner);

        if (ride.Owner.Id == request.PassengerId)
        {
            throw new ActionNotAllowedException("Owner cannot unsubscribe from his ride");
        }

        await _repository.DeleteAsync<UserRide>(userRide.First().Id);
        await _repository.Save();

        return _mapper.Map<Ride, RideShortInfoDTO>(ride);
    }
}