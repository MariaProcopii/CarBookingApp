using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Application.VehicleDetails.Responses;
using CarBookingApp.Application.Vehicles.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.VehicleDetails.Commands;

public class CreateVehicleDetailCommand : IRequest<VehicleDetailDTO>
{
    public int UserId { get; set; }
    public VehicleDTO Vehicle { get; set; }
    public int ManufactureYear { get; set; }
    public string RegistrationNumber { get; set; }
}

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleDetailCommand, VehicleDetailDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;


    public CreateVehicleCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public  async Task<VehicleDetailDTO> Handle(CreateVehicleDetailCommand request, CancellationToken cancellationToken)
    {
        List<Vehicle> vehicle = await _repository.GetByPredicate<Vehicle>(v => v.Vender == request.Vehicle.Vender 
                                                                               && v.Model == request.Vehicle.Model);
        var isRegistrationNumberPresent = await _repository.GetByPredicate<VehicleDetail>(
            vd => vd.RegistrationNumber.Equals(request.RegistrationNumber));
        if (isRegistrationNumberPresent.Count != 0)
        {
            throw new EntityNotValidException("registrationNumber: Registration Number already used.");
        }

        var vehicleDetail = new VehicleDetail
        {
            ManufactureYear = request.ManufactureYear,
            RegistrationNumber = request.RegistrationNumber,
            Vehicle = vehicle.First(),
            Id = request.UserId
        };
        
        vehicleDetail = await _repository.AddAsync(vehicleDetail);
        await _repository.Save();
        
        return _mapper.Map<VehicleDetail, VehicleDetailDTO>(vehicleDetail);
    }
}