using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Commands;

public record DeleteRideCommand(int RideId) : IRequest<RideShortInfoDTO>;

public class DeleteRideCommandHandler : IRequestHandler<DeleteRideCommand, RideShortInfoDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public DeleteRideCommandHandler(IMapper mapper, IRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }


    public async Task<RideShortInfoDTO> Handle(DeleteRideCommand request, CancellationToken cancellationToken)
    {
        var deletedRide = await _repository.DeleteAsyncWithInclude<Ride>(request.RideId, 
            r => r.DestinationFrom, r => r.DestinationTo);
        
        await _repository.Save();

        var rideDto = _mapper.Map<Ride, RideShortInfoDTO>(deletedRide);
        return rideDto;
    }
}