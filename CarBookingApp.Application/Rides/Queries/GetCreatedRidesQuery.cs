using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Models;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Queries;
public record GetCreatedRidesQuery(int UserId, int PageNumber = 1, int PageSize = 9,
    string OrderBy = "DateOfTheRide", bool Ascending = true) : IRequest<PaginatedList<RideCreatedInfoDTO>>;

public class GetCreatedRidesQueryHandler : IRequestHandler<GetCreatedRidesQuery, PaginatedList<RideCreatedInfoDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetCreatedRidesQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RideCreatedInfoDTO>> Handle(GetCreatedRidesQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber;
        int pageSize = request.PageSize;

        var completedUserRides = await _repository.GetByPredicate<UserRide>(
            ur => ur.RideStatus == RideStatus.COMPLETED);
        var completedUserRidesLookup = completedUserRides.ToLookup(ur => ur.RideId, ur => ur);
        
        Expression<Func<Ride, bool>> filter = r => r.Owner.Id.Equals(request.UserId);

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
            r => r.DestinationTo,
            r => r.DestinationFrom,
            r => r.Owner,
            r => r.RideDetail
        );

        var rides = ridesPaginated.Items.Where(r => !completedUserRidesLookup.Contains(r.Id));
        
        var rideDTOs = _mapper.Map<List<RideCreatedInfoDTO>>(rides);

        return new PaginatedList<RideCreatedInfoDTO>(rideDTOs, ridesPaginated.TotalCount, ridesPaginated.PageIndex, ridesPaginated.PageSize);
    }
}