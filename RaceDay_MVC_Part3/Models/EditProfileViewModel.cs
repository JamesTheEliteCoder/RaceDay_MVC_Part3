using System.ComponentModel.DataAnnotations;

namespace RaceDay_MVC_Part3.Models;

public class EditProfileViewModel
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(30, ErrorMessage = "First name cannot exceed 30 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(30, ErrorMessage = "Last name cannot exceed 30 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(11, ErrorMessage = "Phone number cannot exceed 11 characters.")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required.")]
    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }
}