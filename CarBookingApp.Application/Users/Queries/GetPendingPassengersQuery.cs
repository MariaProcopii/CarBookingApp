using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Models;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Queries;
public record GetPendingPassengersQuery(int UserId, int PageNumber = 1, int PageSize = 9, string OrderBy = "Name", bool Ascending = true) : IRequest<PaginatedList<UserDTO>>;

public class GetPendingPassengersQueryHandler : IRequestHandler<GetPendingPassengersQuery, PaginatedList<UserDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetPendingPassengersQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserDTO>> Handle(GetPendingPassengersQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<UserRide, bool>> filter = ur => ur.BookingStatus == BookingStatus.PENDING 
                                                        && ur.Ride.Owner.Id == request.UserId;
        Expression<Func<UserRide, object>> orderBy = null;
        if (!string.IsNullOrEmpty(request.OrderBy))
        {
            orderBy = request.OrderBy.ToLower() switch
            {
                "name" => ur => ur.Passenger.FirstName,
                "email" => ur => ur.Passenger.Email,
                _ => ur => ur.Passenger.FirstName
            };
        }

        var userRidesPaginated = await _repository.GetAllPaginatedAsync(
            pageNumber: request.PageNumber, 
            pageSize: request.PageSize, 
            filter: filter, 
            orderBy: orderBy,
            ascending: request.Ascending ,
            ur => ur.Passenger, ur => ur.Ride.Owner);

        var rides = userRidesPaginated.Items.Select(ur => ur.Passenger).ToList();
        var userDTOs = _mapper.Map<List<User>, List<UserDTO>>(rides);

        return new PaginatedList<UserDTO>(userDTOs, userRidesPaginated.TotalCount, userRidesPaginated.PageIndex, userRidesPaginated.PageSize);
    }
}
