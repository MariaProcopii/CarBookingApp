using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Facilities.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Facilities.Queries;

public record GetAllFacilitiesQuery() : IRequest<List<FacilityDTO>>;

public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, List<FacilityDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetAllFacilitiesQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public  async Task<List<FacilityDTO>> Handle(GetAllFacilitiesQuery request, CancellationToken cancellationToken)
    {
        var facilities = await _repository.GetAllAsync<Facility>();
        return _mapper.Map<IEnumerable<Facility>, List<FacilityDTO>>(facilities);
    }
}