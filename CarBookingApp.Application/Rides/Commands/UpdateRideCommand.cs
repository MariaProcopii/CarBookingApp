using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Commands;

public class UpdateRideCommand : IRequest<RideWithRideDetailsInfoDTO>
{
    public int RideId { get; set; }
    public DateTime DateOfTheRide { get; set; } 
    public string DestinationFrom { get; set; }
    public string DestinationTo { get; set; }
    public int TotalSeats { get; set; }
    public RideDetailDTO RideDetail { get; set; }

}

public class UpdateRideCommandHandler : IRequestHandler<UpdateRideCommand, RideWithRideDetailsInfoDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public UpdateRideCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<RideWithRideDetailsInfoDTO> Handle(UpdateRideCommand request, CancellationToken cancellationToken)
    {
        var rideToUpdate = await _repository.GetByIdWithInclude<Ride>(request.RideId, 
            r => r.DestinationFrom, 
                                r => r.DestinationTo, 
                                r => r.RideDetail,
                                r => r.RideDetail.Facilities);
        
        var destinationFrom = await _repository
            .GetByPredicate<Destination>(d => d.Name == request.DestinationFrom);
        var destinationTo = await _repository
            .GetByPredicate<Destination>(d => d.Name == request.DestinationTo);
        
        List<Facility> newFacilities = new List<Facility>();
  
        foreach (var facilityType in request.RideDetail.Facilities)
        {
            var getFacility = await _repository.GetByPredicate<Facility>(f => f.FacilityType == facilityType);
            newFacilities.Add(getFacility.First());
        }
        
        rideToUpdate.DateOfTheRide = request.DateOfTheRide;
        rideToUpdate.TotalSeats = request.TotalSeats;
        rideToUpdate.DestinationFrom = destinationFrom.First();
        rideToUpdate.DestinationTo = destinationTo.First();
        rideToUpdate.RideDetail.PickUpSpot = request.RideDetail.PickUpSpot;
        rideToUpdate.RideDetail.Price = request.RideDetail.Price;
        rideToUpdate.RideDetail.Facilities = newFacilities;

       var updatedRide = await _repository.UpdateAsync(rideToUpdate);
       await _repository.Save();
       return _mapper.Map<Ride, RideWithRideDetailsInfoDTO>(updatedRide);
    }
}