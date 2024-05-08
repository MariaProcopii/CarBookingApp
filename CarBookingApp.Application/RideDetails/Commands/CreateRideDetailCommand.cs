using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.RideDetails.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.RideDetails.Commands;

public class CreateRideDetailCommand : IRequest<RideDetailDTO>
{
    public int RideId { get; set; }
    public string PickUpSpot { get; set; }
    public decimal Price { get; set; }
    public List<string> Facilities { get; set; } = [];
}

public class CreateRideDetailCommandHandler : IRequestHandler<CreateRideDetailCommand, RideDetailDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public CreateRideDetailCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<RideDetailDTO> Handle(CreateRideDetailCommand request, CancellationToken cancellationToken)
    {
        List<Facility> facilities = new List<Facility>();
        foreach (var facilityType in request.Facilities)
        {
            var getFacility = await _repository.GetByPredicate<Facility>(f => f.FacilityType == facilityType);
            facilities.Add(getFacility.First());
        }

        var rideDetail = new RideDetail
        {
            PickUpSpot = request.PickUpSpot,
            Price = request.Price,
            Facilities = facilities,
            Id = request.RideId
        };

        var createdRideDetail = await _repository.AddAsync(rideDetail);
        await _repository.Save();

        return _mapper.Map<RideDetail, RideDetailDTO>(createdRideDetail);
    }
}



