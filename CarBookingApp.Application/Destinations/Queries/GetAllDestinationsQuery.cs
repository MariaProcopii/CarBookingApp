using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Destinations.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Destinations.Queries;

public record GetAllDestinationsQuery() : IRequest<List<DestinationDTO>>;

public class GetAllDestinationsQueryHandler : IRequestHandler<GetAllDestinationsQuery, List<DestinationDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetAllDestinationsQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public  async Task<List<DestinationDTO>> Handle(GetAllDestinationsQuery request, CancellationToken cancellationToken)
    {
        var destinations = await _repository.GetAllAsync<Destination>();
        return _mapper.Map<IEnumerable<Destination>, List<DestinationDTO>>(destinations);
    }
}
