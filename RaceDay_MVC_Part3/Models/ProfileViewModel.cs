namespace RaceDay_MVC_Part3.Models;

public class ProfileViewModel
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    public string? ProfilePictureUrl { get; set; }


}