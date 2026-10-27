
using System.ComponentModel.DataAnnotations;

namespace RaceDay_MVC_Part3.Models;

// <summary>
/// Holds the information entered on the MVC login page.
/// This model validates form input before credentials are sent to the API.
// </summary>
public class LoginViewModel
{
    // Require a valid email address for the login attempt.
    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(50, ErrorMessage = "The email address cannot exceed 50 characters.")]
    [Display(Name = "Email address")]
    public string Email { get; set; } = string.Empty;

    // Treat the password as sensitive input and never display it as plain text.
    [Required(ErrorMessage = "Please enter your password.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}

