using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Commands;

public class UpdateRideCommand : IRequest<RideDTO>
{
    public DateTime DateOfTheRide { get; set; } 
    public string DestinationFrom { get; set; }
    public string DestinationTo { get; set; }
    public int TotalSeats { get; set; }
    public int RideId { get; set; }
}

public class UpdateRideCommandHandler : IRequestHandler<UpdateRideCommand, RideDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public UpdateRideCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<RideDTO> Handle(UpdateRideCommand request, CancellationToken cancellationToken)
    {
        var rideToUpdate = await _repository.GetByIdWithInclude<Ride>(request.RideId, 
            r => r.DestinationFrom, r => r.DestinationTo);
        
        var destinationFrom = await _repository
            .GetByPredicate<Destination>(d => d.Name == request.DestinationFrom);
        var destinationTo = await _repository
            .GetByPredicate<Destination>(d => d.Name == request.DestinationTo);

        rideToUpdate.DateOfTheRide = request.DateOfTheRide;
        rideToUpdate.TotalSeats = request.TotalSeats;
        rideToUpdate.DestinationFrom = destinationFrom.First();
        rideToUpdate.DestinationTo = destinationTo.First();

       var updatedRide = await _repository.UpdateAsync(rideToUpdate);
       await _repository.Save();
       return _mapper.Map<Ride, RideDTO>(updatedRide);
    }
}