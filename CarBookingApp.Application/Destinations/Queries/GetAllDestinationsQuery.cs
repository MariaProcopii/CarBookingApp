using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Destinations.Queries;

public record GetAllDestinationsQuery() : IRequest<List<String>>;

public class GetAllDestinationsQueryHandler : IRequestHandler<GetAllDestinationsQuery, List<String>>
{
    private readonly IRepository _repository;

    public GetAllDestinationsQueryHandler(IRepository repository)
    {
        _repository = repository;
    }
    
    public  async Task<List<String>> Handle(GetAllDestinationsQuery request, CancellationToken cancellationToken)
    {
        var destinations = await _repository.GetAllAsync<Destination>();
        var destinationsAsString = destinations.Select(d => d.Name).ToList();
        return destinationsAsString;
    }
}