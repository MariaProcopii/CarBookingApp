using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Vehicles.Queries;

public record GetAllUniqueVendorQuery : IRequest<List<string>>;

public class GetAllUniqueVendorQueryHandler : IRequestHandler<GetAllUniqueVendorQuery, List<String>>
{
    private readonly IRepository _repository;

    public GetAllUniqueVendorQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<String>> Handle(GetAllUniqueVendorQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _repository.GetAllAsync<Vehicle>();
        return vehicles.Select(v => v.Vender).Distinct().ToList();
    }
}
