using System.ComponentModel.DataAnnotations;

namespace RaceDay_MVC_Part3.Models;

public class UploadProfilePictureViewModel
{
    [Required(ErrorMessage = "Please select a profile picture.")]
    [Display(Name = "Choose Profile Picture")]
    public IFormFile? ProfilePicture { get; set; }
}