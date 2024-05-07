using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Queries;
using CarBookingApp.Application.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

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
    [Route("info/{id}")]
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
    [Route("create")]
    public async Task<UserDTO> CreateUser([FromBody] CreateUserCommand createUserCommand)
    {
        var result = await _mediator.Send(createUserCommand);
        return result;
    }
    
    [HttpPut]
    [Route("info/update/{id}")]
    public async Task<UserDTO> UpdateUser(int id, [FromBody] UpdateUserCommand updateUserCommand)
    {
        updateUserCommand.Id = id;
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
    
    [HttpPost]
    [Route("info/upgrade/{id}")]
    public async Task<UserDTO> UpgradeUserToDriver(int id, [FromBody] UpgradeUserToDriverCommand upgradeUserToDriverCommand)
    {
        upgradeUserToDriverCommand.Id = id;
        var result = await _mediator.Send(upgradeUserToDriverCommand);
        return result;
    }
    
    [HttpPut]
    [Route("info/downgrade/{id}")]
    public async Task<UserDTO> DowngradeDriverToUser(int id)
    {
        var result = await _mediator.Send(new DowngradeDriverToUserCommand(id));
        return result;
    }
}