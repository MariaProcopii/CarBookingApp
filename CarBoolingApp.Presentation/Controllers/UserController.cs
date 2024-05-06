using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Queries;
using CarBookingApp.Application.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBoolingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IMediator _mediator;

    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<UserDTO> GetUserById(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return result;
    }
    
    [HttpGet]
    public async Task<List<UserDTO>> GetUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return result;
    }
    
    [HttpPost]
    public async Task<UserDTO> CreateUser([FromBody] CreateUserCommand createUserCommand)
    {
        var result = await _mediator.Send(createUserCommand);
        return result;
    }
    
    [HttpPut]
    public async Task<UserDTO> UpdateUser([FromBody] UpdateUserCommand updateUserCommand)
    {
        var result = await _mediator.Send(updateUserCommand);
        return result;
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<UserDTO> DeleteUser(int id)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id));
        return result;
    }
}