using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Models;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Queries;

public record GetAllRidesQuery(int UserId, int PageNumber = 1, int PageSize = 9, 
    string OrderBy = "DateOfTheRide", bool Ascending = true) : IRequest<PaginatedList<RideShortInfoDTO>>;

public class GetAllRidesQueryHandler : IRequestHandler<GetAllRidesQuery, PaginatedList<RideShortInfoDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRidesQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RideShortInfoDTO>> Handle(GetAllRidesQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber;
        int pageSize = request.PageSize;

        Expression<Func<Ride, bool>> filter = r => r.Owner.Id != request.UserId 
                                                   && r.DateOfTheRide > DateTime.UtcNow 
                                                   && r.Passengers.Count < r.TotalSeats;

        Expression<Func<Ride, object>> orderBy = request.OrderBy.ToLower() switch
        {
            "dateoftheride" => r => r.DateOfTheRide,
            "destinationfrom" => r => r.DestinationFrom,
            "destinationto" => r => r.DestinationTo,
            _ => r => r.DateOfTheRide
        };

        var ridesPaginated = await _repository.GetAllPaginatedAsync(
            pageNumber: pageNumber,
            pageSize: pageSize,
            filter: filter,
            orderBy: orderBy,
            ascending: request.Ascending,
            r => r.DestinationFrom,
            r => r.DestinationTo,
            r => r.Passengers
        );

        var rideDTOs = _mapper.Map<List<RideShortInfoDTO>>(ridesPaginated.Items);

        return new PaginatedList<RideShortInfoDTO>(rideDTOs, ridesPaginated.TotalCount, ridesPaginated.PageIndex, ridesPaginated.PageSize);
    }
}
