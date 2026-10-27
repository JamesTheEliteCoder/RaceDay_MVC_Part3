
namespace RaceDay_MVC_Part3.Models;


public class ApiUserResponse
{
    // Unique identifier assigned to the user by the API database.
    public int UserId { get; set; }

    // The user's personal details returned by the API.
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // Account details used by the MVC application.
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    // Additional profile information returned by the API.
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
}
