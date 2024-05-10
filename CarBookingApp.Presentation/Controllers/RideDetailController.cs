// using CarBookingApp.Application.RideDetails.Commands;
// using CarBookingApp.Application.RideDetails.Responses;
// using MediatR;
// using Microsoft.AspNetCore.Mvc;
//
// namespace CarBookingApp.Presentation.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// public class RideDetailController : ControllerBase
// {
//     private readonly ILogger<UserController> _logger;
//
//     private readonly IMediator _mediator;
//
//     public RideDetailController(ILogger<UserController> logger, IMediator mediator)
//     {
//         _logger = logger;
//         _mediator = mediator;
//     }
//
//     [HttpPost]
//     [Route("create/{rideId}")]
//     public async Task<RideDetailDTO> CreateRideDetail(int rideId,
//         [FromBody] CreateRideDetailCommand createRideDetailCommand)
//     {
//         createRideDetailCommand.RideId = rideId;
//         return await _mediator.Send(createRideDetailCommand);
//     }
//     
//     [HttpPut]
//     [Route("info/update/{rideId}")]
//     public async Task<RideDetailDTO> UpdateRideDetail(int rideId,
//         [FromBody] UpdateRideDetailCommand updateRideDetailCommand)
//     {
//         updateRideDetailCommand.RideId = rideId;
//         return await _mediator.Send(updateRideDetailCommand);
//     }
// }