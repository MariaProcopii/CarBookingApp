using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Queries;

public record GetAllUsersQuery() : IRequest<List<UserDTO>>;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
{
    private readonly IUnitOfWork<User> _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUnitOfWork<User> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.EntityRepository.GetAllAsync();
        var userDTOs = _mapper.Map<IEnumerable<User>, List<UserDTO>>(users);
        return userDTOs;
    }
}