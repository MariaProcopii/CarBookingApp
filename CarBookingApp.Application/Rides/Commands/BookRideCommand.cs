using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Commands;

public class BookRideCommand() : IRequest<RideDTO>
{
    public int RideId { get; set; }
    public int PassengerId { get; set; }
}

public class BookRideCommandHandler : IRequestHandler<BookRideCommand, RideDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public BookRideCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RideDTO> Handle(BookRideCommand request, CancellationToken cancellationToken)
    {
        var bookings = await _repository.GetByPredicate<UserRide>(ur => ur.RideId == request.RideId);
        var ride = await _repository.GetByIdWithInclude<Ride>(request.RideId, 
            r => r.DestinationFrom, r => r.DestinationTo, r => r.Owner);
        
        if (ride.TotalSeats <= bookings.Count)
        {
            throw new Exception("No available seats");
        }
        
        if (ride.Owner.Id == request.PassengerId)
        {
            throw new Exception("Owner cannot book his ride");
        }

        var userRide = new UserRide()
        {
            PassengerId = request.PassengerId,
            RideId = request.RideId
        };

        await _repository.AddAsync(userRide);
        await _repository.Save();
        return _mapper.Map<Ride, RideDTO>(ride);
    }
}