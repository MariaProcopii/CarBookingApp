using System.Linq.Expressions;
using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Models;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Queries;

public record GetAllUsersQuery(int PageNumber = 1, int PageSize = 9, string? Username = null, string OrderBy = "Name", bool Ascending = true) : IRequest<PaginatedList<UserDTO>>;


public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PaginatedList<UserDTO>>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber;
        int pageSize = request.PageSize;

        Expression<Func<User, bool>> filter = null;
        if (!string.IsNullOrEmpty(request.Username))
        {
            filter = user => user.FirstName == request.Username;
        }
        
        Expression<Func<User, object>> orderBy = request.OrderBy.ToLower() switch
        {
            "name" => user => user.LastName,
            "email" => user => user.Email,
            _ => user => user.FirstName
        };

        var usersPaginated = await _repository.GetAllPaginatedAsync(
            pageNumber: pageNumber,
            pageSize: pageSize,
            filter: filter,
            orderBy: orderBy,
            request.Ascending
        );
        var userDTOs = _mapper.Map<List<UserDTO>>(usersPaginated.Items);

        return new PaginatedList<UserDTO>(userDTOs, usersPaginated.TotalCount, usersPaginated.PageIndex, usersPaginated.PageSize);
    }
}