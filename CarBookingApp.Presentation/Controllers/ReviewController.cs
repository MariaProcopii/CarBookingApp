using CarBookingApp.Application.Review.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingApp.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "User, Driver")]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}