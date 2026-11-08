using Microsoft.AspNetCore.Mvc;
using RaceDay_MVC_Part3.Services;

namespace RaceDay_MVC_Part3.Controllers;

public class ImagesController : Controller
{
    private readonly RaceDayApiClient _apiClient;

    // Receive the API client through dependency injection.
    public ImagesController(RaceDayApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Retrieves an event banner for display in the browser.
    [HttpGet]
    public async Task<IActionResult> EventBanner(int eventId)
    {
        // Ensure that the visitor has an active MVC session.
        if (HttpContext.Session.GetInt32("UserId") is null)
        {
            return Unauthorized();
        }

        // Retrieve the banner through the Part 2 API.
        var image = await _apiClient.GetEventBannerAsync(
            eventId,
            HttpContext.Session);

        // Return 404 if the event or banner does not exist.
        if (image is null)
        {
            return NotFound();
        }

        // Send the image bytes to the browser.
        return File(image.Value.ImageData, image.Value.ContentType);
    }// end of EventBanner method




    // Retrieves the signed-in user's profile picture.
    [HttpGet]
    public async Task<IActionResult> ProfilePicture()
    {
        // Ensure that the visitor has an active MVC session.
        if (HttpContext.Session.GetInt32("UserId") is null)
        {
            return Unauthorized();
        }

        // Retrieve the profile picture through the Part 2 API.
        var image = await _apiClient.GetMyProfilePictureAsync(
            HttpContext.Session);

        // Return 404 if the user has no profile picture.
        if (image is null)
        {
            return NotFound();
        }

        // Send the image bytes to the browser.
        return File(image.Value.ImageData, image.Value.ContentType);
    }// end of ProfilePicture method











}//end of ImagesController class