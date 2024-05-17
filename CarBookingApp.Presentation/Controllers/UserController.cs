using CarBookingApp.Application.Users.Commands;
using CarBookingApp.Application.Users.Queries;
using CarBookingApp.Application.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("info/{id}")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<UserDTO>> GetUserById(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(result);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<UserDTO>>> GetUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<ActionResult<UserDTO>> CreateUser(
        [FromBody] CreateUserCommand createUserCommand)
    {
        var result = await _mediator.Send(createUserCommand);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("info/update/{id}")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<UserDTO>> UpdateUser(int id, 
        [FromBody] UpdateUserCommand updateUserCommand)
    {
        updateUserCommand.Id = id;
        var result = await _mediator.Send(updateUserCommand);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<UserDTO>> DeleteUser(int id)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id));
        return Ok(result);
    }
    
    [HttpPost]
    [Route("info/upgrade/{id}")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<UserDTO>> UpgradeUserToDriver(int id, 
        [FromBody] UpgradeUserToDriverCommand upgradeUserToDriverCommand)
    {
        upgradeUserToDriverCommand.Id = id;
        var result = await _mediator.Send(upgradeUserToDriverCommand);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("info/downgrade/{id}")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<UserDTO>> DowngradeDriverToUser(int id)
    {
        var result = await _mediator.Send(new DowngradeDriverToUserCommand(id));
        return Ok(result);
    }
    
    [HttpPut]
    [Route("pending/approve")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<int>> ApproveUserForRide(
        [FromBody] ApproveUserForRideCommand approveUserForRideCommand)
    {
        var result = await _mediator.Send(approveUserForRideCommand);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("pending/reject")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<int>> RejectUserForRide(
        [FromBody] RejectUserForRideCommand rejectUserForRideCommand)
    {
        var result =await _mediator.Send(rejectUserForRideCommand);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("pending/{userId}")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<List<UserDTO>>> GetPendingPassengers(int userId)
    {
        var result = await _mediator.Send(new GetPendingPassengersQuery(userId));
        return Ok(result);
    }
}