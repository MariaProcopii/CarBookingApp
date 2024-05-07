namespace CarBookingApp.Domain.Model;

public class RideReview
{
    public int Id { get; set; }
    public User Reviewer { get; set; }
    public User Reviewee { get; set; }
    public float Rating { get; set; }
    public string ReviewComment { get; set; }
}