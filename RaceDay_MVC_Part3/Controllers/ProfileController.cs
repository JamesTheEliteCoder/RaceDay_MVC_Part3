using Microsoft.AspNetCore.Mvc;
using RaceDay_MVC_Part3.Models;
using RaceDay_MVC_Part3.Services;

namespace RaceDay_MVC_Part3.Controllers;

public class ProfileController : Controller
{
    private readonly RaceDayApiClient _apiClient;

    public ProfileController(RaceDayApiClient apiClient)
    {
        _apiClient = apiClient;
    }


// Displays the profile of the currently signed-in participant.
[HttpGet]
    public async Task<IActionResult> Index()
    {
        // Confirm that the participant is logged in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only participants may access this profile page for now.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Participant", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            // Retrieve the participant's profile through the Part 2 API.
            var profile = await _apiClient.GetMyProfileAsync(
                HttpContext.Session);

            if (profile is null)
            {
                return NotFound("Your profile could not be found.");
            }

            return View(profile);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error retrieving participant profile: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Unable to retrieve your profile at this time.");
        }// end of try-catch block
    }// end of Index method




    // Displays the profile editing form.
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Participant", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            var profile = await _apiClient.GetMyProfileAsync(
                HttpContext.Session);

            if (profile is null)
            {
                return NotFound("Your profile could not be found.");
            }

            var model = new EditProfileViewModel
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                DateOfBirth = profile.DateOfBirth
            };

            return View(model);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error retrieving profile for editing: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Unable to retrieve your profile at this time.");
        }
    }


    // Processes the submitted profile changes.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProfileViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Participant", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _apiClient.UpdateMyProfileAsync(
                model,
                HttpContext.Session);

            TempData["SuccessMessage"] = "Your profile was updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error updating participant profile: {ex.Message}");

            ModelState.AddModelError(
                string.Empty,
                "Unable to update your profile. Please try again.");

            return View(model);
        }
    }// end of Edit (POST) method








    // Processes a participant's profile-picture upload.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadProfilePicture(
        UploadProfilePictureViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Participant", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please select a profile picture.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            await _apiClient.UploadProfilePictureAsync(
                model.ProfilePicture!,
                HttpContext.Session);

            TempData["SuccessMessage"] =
                "Your profile picture was uploaded successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error uploading profile picture: {ex.Message}");

            TempData["ErrorMessage"] =
                "Unable to upload your profile picture. Please try again.";

            return RedirectToAction(nameof(Index));
        }
    }// end of UploadProfilePicture method

}// end of ProfileController class