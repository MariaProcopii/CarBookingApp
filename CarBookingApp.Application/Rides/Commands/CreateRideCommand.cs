using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Commands;

public class CreateRideCommand : IRequest<RideWithRideDetailsInfoDTO>
{
    public DateTime DateOfTheRide { get; set; } 
    public string DestinationFrom { get; set; }
    public string DestinationTo { get; set; }
    public int TotalSeats { get; set; }
    public int OwnerId { get; set; }
    public RideDetailDTO RideDetail { get; set; }
}

public class CreateRideCommandHandler : IRequestHandler<CreateRideCommand, RideWithRideDetailsInfoDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public CreateRideCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RideWithRideDetailsInfoDTO> Handle(CreateRideCommand request, CancellationToken cancellationToken)
    {
        var owner = await _repository.GetByIdAsync<User>(request.OwnerId);
        if (owner is not Driver)
        {
            throw new ActionNotAllowedException("User is not a driver.");
        }
        var destinationFrom = await _repository
            .GetByPredicate<Destination>(d => d.Name == request.DestinationFrom);
        var destinationTo = await _repository
            .GetByPredicate<Destination>(d => d.Name == request.DestinationTo);
        
        List<Facility> facilities = new List<Facility>();
        foreach (var facilityType in request.RideDetail.Facilities)
        {
            var getFacility = await _repository.GetByPredicate<Facility>(f => f.FacilityType == facilityType);
            facilities.Add(getFacility.First());
        }

        var rideDetail = new RideDetail
        {
            PickUpSpot = request.RideDetail.PickUpSpot,
            Price = request.RideDetail.Price,
            Facilities = facilities
        };

        var ride = new Ride
        {
            DateOfTheRide = request.DateOfTheRide,
            TotalSeats = request.TotalSeats,
            DestinationFrom = destinationFrom.First(),
            DestinationTo = destinationTo.First(),
            Owner = (Driver)owner,
            RideDetail = rideDetail
        };

        var createdRide = await _repository.AddAsync(ride);
        await _repository.Save();
        
        return _mapper.Map<Ride, RideWithRideDetailsInfoDTO>(createdRide);
    }
}