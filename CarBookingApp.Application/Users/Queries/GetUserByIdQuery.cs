using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Responses;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Users.Queries;

public class GetUserByIdQuery : IRequest<UserDTO>
{
    public int Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
{
    private readonly IUnitOfWork<User> _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUnitOfWork<User> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.EntityRepository.GetByIdAsync(request.Id);
        return _mapper.Map<User, UserDTO>(user);
    }
}