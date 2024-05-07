using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.VehicleDetails.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.VehicleDetails.Queries;

public record GetVehicleDetailByIdQuery(int UserId) : IRequest<VehicleDetailDTO>;

public class GetVehicleDetailByIdQueryHandler : IRequestHandler<GetVehicleDetailByIdQuery, VehicleDetailDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetVehicleDetailByIdQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VehicleDetailDTO> Handle(GetVehicleDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicleDetail = await _repository.GetByIdWithInclude<VehicleDetail>(request.UserId, vd => vd.Vehicle);
        return _mapper.Map<VehicleDetail, VehicleDetailDTO>(vehicleDetail);
    }
}