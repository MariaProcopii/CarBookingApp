using AutoMapper;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using MediatR;

namespace CarBookingApp.Application.Review.Commands;

public class CreateReviewCommand : IRequest
{
    public int RideReviewerId { get; set; }
    public int RideRevieweeId { get; set; } 
    public float Rating { get; set; }
    public string ReviewComment { get; set; }
}

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var reviewer = await _repository.GetByIdAsync<User>(request.RideReviewerId);
        var reviewee = await _repository.GetByIdAsync<User>(request.RideRevieweeId);

        var review = new RideReview
        {
            Reviewer = reviewer,
            Reviewee = reviewee,
            Rating = request.Rating,
            ReviewComment = request.ReviewComment
        };

        await _repository.AddAsync(review);
        await _repository.Save();
    }
}