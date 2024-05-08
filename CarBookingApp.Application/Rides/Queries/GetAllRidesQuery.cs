using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Queries;

public record GetAllRidesQuery(int UserId) : IRequest<List<RideDTO>>;

public class GetAllRidesQueryHandler : IRequestHandler<GetAllRidesQuery, List<RideDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRidesQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<RideDTO>> Handle(GetAllRidesQuery request, CancellationToken cancellationToken)
    {
        var rides = await _repository.GetByPredicate<Ride>(r => r.Owner.Id != request.UserId, 
            r => r.DestinationFrom, r => r.DestinationTo);
        
        return _mapper.Map<List<Ride>, List<RideDTO>>(rides);
    }
}