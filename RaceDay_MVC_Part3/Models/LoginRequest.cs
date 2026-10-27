
using System.ComponentModel.DataAnnotations;

namespace RaceDay_MVC_Part3.Models;


public class LoginRequest
{
    // Require a valid email address before sending the login request.
    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    // The API verifies the password against its stored password hash.
    // MVC must never store the user's password in session.
    [Required(ErrorMessage = "Please enter your password.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
