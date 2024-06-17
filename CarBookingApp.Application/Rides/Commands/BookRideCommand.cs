using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Commands;

public class BookRideCommand() : IRequest<RideShortInfoDTO>
{
    public int RideId { get; set; }
    public int PassengerId { get; set; }
}

public class BookRideCommandHandler : IRequestHandler<BookRideCommand, RideShortInfoDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public BookRideCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RideShortInfoDTO> Handle(BookRideCommand request, CancellationToken cancellationToken)
    {
        var ride = await _repository.GetByIdWithInclude<Ride>(request.RideId, 
            r => r.DestinationFrom, 
                                r => r.DestinationTo,
                                r => r.Owner,
                                r => r.Passengers);
        
        var approvedUserRides = await _repository.GetByPredicate<UserRide>(
            ur => ur.BookingStatus == BookingStatus.APPROVED && ur.Ride.Id == request.RideId);
        
        if (ride.TotalSeats <= approvedUserRides.Count)
        {
            throw new ActionNotAllowedException("No available seats");
        }
        
        if (ride.Owner.Id == request.PassengerId)
        {
            throw new ActionNotAllowedException("Owner cannot book his ride");
        }
        
        Console.WriteLine($">>>>>>>>>>>{request.PassengerId}>>>>>>>>>>>>{request.RideId}");

        var userRide = new UserRide()
        {
            PassengerId = request.PassengerId,
            RideId = request.RideId
        };

        await _repository.AddAsync(userRide);
        await _repository.Save();
        return _mapper.Map<Ride, RideShortInfoDTO>(ride);
    }
}