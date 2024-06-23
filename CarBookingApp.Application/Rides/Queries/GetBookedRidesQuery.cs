using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Models;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Queries;

public record GetBookedRidesQuery(int UserId, int PageNumber = 1, int PageSize = 9,
    string OrderBy = "DateOfTheRide", bool Ascending = true) : IRequest<PaginatedList<RideShortInfoDTO>>;

public class GetBookedRidesQueryHandler : IRequestHandler<GetBookedRidesQuery, PaginatedList<RideShortInfoDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetBookedRidesQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RideShortInfoDTO>> Handle(GetBookedRidesQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber;
        int pageSize = request.PageSize;
        
        Expression<Func<UserRide, bool>> filter = ur => ur.BookingStatus == BookingStatus.APPROVED
                                                        && ur.RideStatus == RideStatus.UPCOMING
                                                        && ur.PassengerId == request.UserId;

        Expression<Func<UserRide, object>> orderBy = request.OrderBy.ToLower() switch
        {
            "dateoftheride" => ur => ur.Ride.DateOfTheRide,
            "destinationfrom" => ur => ur.Ride.DestinationFrom,
            "destinationto" => ur => ur.Ride.DestinationTo,
            _ => ur => ur.Ride.DateOfTheRide
        };

        var userRidesPaginated = await _repository.GetAllPaginatedAsync(
            pageNumber: pageNumber,
            pageSize: pageSize,
            filter: filter,
            orderBy: orderBy,
            ascending: request.Ascending,
            ur => ur.Ride,
            ur => ur.Ride.DestinationTo,
            ur => ur.Ride.DestinationFrom
        );

        var rides = userRidesPaginated.Items.Select(ur => ur.Ride).ToList();
        
        var rideDTOs = _mapper.Map<List<RideShortInfoDTO>>(rides);

        return new PaginatedList<RideShortInfoDTO>(rideDTOs, userRidesPaginated.TotalCount, userRidesPaginated.PageIndex, userRidesPaginated.PageSize);
    }
}