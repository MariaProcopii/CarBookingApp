using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Models;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Rides.Queries;

public record GetAllRidesQuery(
    int UserId,
    int PageNumber = 1,
    int PageSize = 8,
    string OrderBy = "DateOfTheRide",
    bool Ascending = true,
    DateTime? DateOfTheRide = null,
    string? DestinationFrom = null,
    string? DestinationTo = null,
    int TotalSeats = 0
) : IRequest<PaginatedList<RideShortInfoDTO>>;

public class GetAllRidesQueryHandler : IRequestHandler<GetAllRidesQuery, PaginatedList<RideShortInfoDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRidesQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RideShortInfoDTO>> Handle(GetAllRidesQuery request,
        CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber;
        int pageSize = request.PageSize;

        Expression<Func<Ride, bool>> filter = r => r.Owner.Id != request.UserId
                                                   && r.DateOfTheRide > DateTime.Now
                                                   && !r.Passengers.Select(p => p.Id).Contains(request.UserId);

        if (!string.IsNullOrEmpty(request.DestinationFrom))
        {
            filter = filter.AndAlso(r => r.DestinationFrom.Name == request.DestinationFrom);
        }

        if (!string.IsNullOrEmpty(request.DestinationTo))
        {
            filter = filter.AndAlso(r => r.DestinationTo.Name == request.DestinationTo);
        }
        
        if (request.TotalSeats != 0)
        {
            filter = filter.AndAlso(r => r.TotalSeats == request.TotalSeats);
        }

        if (request.DateOfTheRide.HasValue)
        {
            filter = filter.AndAlso(r => r.DateOfTheRide.Date == request.DateOfTheRide);
        }

        Expression<Func<Ride, object>> orderBy = request.OrderBy.ToLower() switch
        {
            "dateoftheride" => r => r.DateOfTheRide,
            "destinationfrom" => r => r.DestinationFrom,
            "destinationto" => r => r.DestinationTo,
            _ => r => r.DateOfTheRide
        };

        var rides = await _repository.GetAllPaginatedAsync(
            pageNumber: pageNumber,
            pageSize: pageSize,
            filter: filter,
            orderBy: orderBy,
            ascending: request.Ascending,
            r => r.DestinationFrom,
            r => r.DestinationTo,
            r => r.Passengers,
            r => r.Owner,
            r => r.RideDetail
        );

        var rideDTOs = _mapper.Map<List<RideShortInfoDTO>>(rides.Items);

        return new PaginatedList<RideShortInfoDTO>(rideDTOs, rides.TotalCount, rides.PageIndex, rides.PageSize);
    }
}

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(expr1, parameter),
            Expression.Invoke(expr2, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}