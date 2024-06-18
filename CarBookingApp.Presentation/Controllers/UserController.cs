using CarBookingApp.Application.Common.Models;
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
    [Route("info")]
    [Authorize(Roles = "User, Driver")]
    public async Task<ActionResult<UserDTO>> GetUserByEmail([FromBody] GetUserByEmailQuery getUserByEmailQuery)
    {
        var result = await _mediator.Send(getUserByEmailQuery);
        return Ok(result);
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
    public async Task<ActionResult<PaginatedList<UserDTO>>> GetUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 9,
        [FromQuery] string? username = null,
        [FromQuery] string orderBy = "Name",
        [FromQuery] bool ascending = true)
    {
        var query = new GetAllUsersQuery(pageNumber, pageSize, username, orderBy, ascending);
        var result = await _mediator.Send(query);
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
    public async Task<ActionResult<int>> DeleteUser(int id)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id));
        return Ok(result);
    }
    
    [HttpPost]
    [Route("info/upgrade/{id}")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<string>> UpgradeUserToDriver(int id, 
        [FromBody] UpgradeUserToDriverCommand upgradeUserToDriverCommand)
    {
        upgradeUserToDriverCommand.Id = id;
        var result = await _mediator.Send(upgradeUserToDriverCommand);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("info/downgrade/{id}")]
    [Authorize(Roles = "Driver")]
    public async Task<ActionResult<string>> DowngradeDriverToUser(int id)
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
    public async Task<ActionResult<PaginatedList<PendingUserDTO>>> GetPendingPassengers(
        int userId, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string orderBy = "Name", 
        [FromQuery] bool ascending = true)
    {
        var query = new GetPendingPassengersQuery(userId, pageNumber, pageSize, orderBy, ascending);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}