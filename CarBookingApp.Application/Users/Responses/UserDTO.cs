using Newtonsoft.Json;
namespace CarBookingApp.Application.Users.Responses;

public class UserDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? YearsOfExperience { get; set; }
}