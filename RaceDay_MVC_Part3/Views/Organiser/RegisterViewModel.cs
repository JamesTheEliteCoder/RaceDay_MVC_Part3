using System.ComponentModel.DataAnnotations;

namespace RaceDay_MVC_Part3.Models;

public class RegisterViewModel
{
    // User's personal details.
    [Required]
    [StringLength(30)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string LastName { get; set; } = string.Empty;

    // Login credentials.
    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    // Determines which interface the user accesses.
    [Required]
    [RegularExpression("^(Organiser|Participant)$",
        ErrorMessage = "Please select a valid account role.")]
    public string Role { get; set; } = string.Empty;

    // Additional contact and personal information.
    [Required]
    [StringLength(11)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }
}