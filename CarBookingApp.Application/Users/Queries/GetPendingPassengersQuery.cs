using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Models;
using CarBookingApp.Application.Rides.Responses;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Enum;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Queries;
public record GetPendingPassengersQuery(int UserId, int PageNumber = 1, int PageSize = 9, string OrderBy = "Name", bool Ascending = true) : IRequest<PaginatedList<PendingUserDTO>>;

public class GetPendingPassengersQueryHandler : IRequestHandler<GetPendingPassengersQuery, PaginatedList<PendingUserDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetPendingPassengersQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PendingUserDTO>> Handle(GetPendingPassengersQuery request, CancellationToken cancellationToken)
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
            ur => ur.Passenger,
            ur => ur.Ride,
            ur => ur.Ride.DestinationFrom,
            ur => ur.Ride.DestinationTo);

        var pendingUserDtos = userRidesPaginated.Items.Select(ur => new PendingUserDTO(){
                UserInfo = _mapper.Map<User, UserDTO>(ur.Passenger), 
                RideInfo = _mapper.Map<Ride, RideCreatedInfoDTO>(ur.Ride)
        }).ToList();

        return new PaginatedList<PendingUserDTO>(pendingUserDtos, userRidesPaginated.TotalCount, userRidesPaginated.PageIndex, userRidesPaginated.PageSize);
    }
}
