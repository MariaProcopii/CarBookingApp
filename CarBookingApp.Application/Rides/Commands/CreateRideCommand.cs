using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Commands;

public class CreateRideCommand : IRequest<RideDTO>
{
    public DateTime DateOfTheRide { get; set; } 
    public string DestinationFrom { get; set; }
    public string DestinationTo { get; set; }
    public int TotalSeats { get; set; }
    public int OwnerId { get; set; }
}

public class CreateRideCommandHandler : IRequestHandler<CreateRideCommand, RideDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public CreateRideCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RideDTO> Handle(CreateRideCommand request, CancellationToken cancellationToken)
    {
        var owner = await _repository.GetByIdAsync<User>(request.OwnerId);
        if (owner is not Driver)
        {
            throw new Exception("User is not a driver");
        }
        var destinationFrom = await _repository
            .GetByPredicate<Destination>(d => d.Name == request.DestinationFrom);
        var destinationTo = await _repository
            .GetByPredicate<Destination>(d => d.Name == request.DestinationTo);

        var ride = new Ride
        {
            DateOfTheRide = request.DateOfTheRide,
            TotalSeats = request.TotalSeats,
            DestinationFrom = destinationFrom.First(),
            DestinationTo = destinationTo.First(),
            Owner = (Driver)owner
        };

        var createdRide = await _repository.AddAsync(ride);
        await _repository.Save();
        return _mapper.Map<Ride, RideDTO>(createdRide);
    }
}