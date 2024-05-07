using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.VehicleDetails.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.VehicleDetails.Commands;

public class UpdateVehicleDetailCommand : IRequest<VehicleDetailDTO>
{
    public int UserId { get; set; }
    public string Vender { get; set; }
    public string Model { get; set; }
    public int ManufactureYear { get; set; }
    public string RegistrationNumber { get; set; }
}

public class UpdateVehicleDetailCommandHandler : IRequestHandler<UpdateVehicleDetailCommand, VehicleDetailDTO>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;


    public UpdateVehicleDetailCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public  async Task<VehicleDetailDTO> Handle(UpdateVehicleDetailCommand request, CancellationToken cancellationToken)
    {
        List<Vehicle> vehicle = await _repository.GetByPredicate<Vehicle>(v => v.Vender == request.Vender 
                                                                               && v.Model == request.Model);

        var vehicleDetailToUpdate = await _repository.GetByIdAsync<VehicleDetail>(request.UserId);
        
        vehicleDetailToUpdate.ManufactureYear = request.ManufactureYear;
        vehicleDetailToUpdate.RegistrationNumber = request.RegistrationNumber;
        vehicleDetailToUpdate.Vehicle = vehicle.First();
        
        var updatedVehicleDetail = await _repository.UpdateAsync(vehicleDetailToUpdate);
        await _repository.Save();
        
        return _mapper.Map<VehicleDetail, VehicleDetailDTO>(updatedVehicleDetail);
    }
}