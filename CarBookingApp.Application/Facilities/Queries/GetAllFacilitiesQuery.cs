using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Facilities.Queries;

public record GetAllFacilitiesQuery() : IRequest<List<String>>;

public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, List<String>>
{
    private readonly IRepository _repository;

    public GetAllFacilitiesQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public  async Task<List<String>> Handle(GetAllFacilitiesQuery request, CancellationToken cancellationToken)
    {
        var facilities = await _repository.GetAllAsync<Facility>();
        var facilitiesAsString = facilities.Select(f => f.FacilityType).ToList();
        return facilitiesAsString;
    }
}