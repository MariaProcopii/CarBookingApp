using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.RideDetails.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.RideDetails.Commands;

public class UpdateRideDetailCommand : IRequest<RideDetailDTO>
{
    public int RideId { get; set; }
    public string PickUpSpot { get; set; }
    public decimal Price { get; set; }
    public List<string> Facilities { get; set; } = [];
}

public class UpdateRideDetailCommandHandler : IRequestHandler<UpdateRideDetailCommand, RideDetailDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public UpdateRideDetailCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RideDetailDTO> Handle(UpdateRideDetailCommand request, CancellationToken cancellationToken)
    {
        List<Facility> newFacilities = new List<Facility>();
        var rideDetailToUpdate = await _repository.GetByIdWithInclude<RideDetail>(request.RideId,
            rd => rd.Facilities);
        
        foreach (var facilityType in request.Facilities)
        {
            var getFacility = await _repository.GetByPredicate<Facility>(f => f.FacilityType == facilityType);
            newFacilities.Add(getFacility.First());
        }
        
        rideDetailToUpdate.PickUpSpot = request.PickUpSpot;
        rideDetailToUpdate.Price = request.Price;
        rideDetailToUpdate.Facilities = newFacilities;

        var updatedRideDetail = await _repository.UpdateAsync(rideDetailToUpdate);
        await _repository.Save();

        return _mapper.Map<RideDetail, RideDetailDTO>(updatedRideDetail);
    }
}