using RaceDay_MVC_Part3.Models;
using RaceDay_MVC_Part3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RaceDay_MVC_Part3.Controllers;


public class ParticipantController : Controller
{

    private readonly RaceDayApiClient _apiClient;

    public ParticipantController(RaceDayApiClient apiClient)
    {
        // Provides access to the Part 2 API.
        _apiClient = apiClient;
    }






    //Displays the participant landing page to authenticated participants.
    [HttpGet]
    public IActionResult Index()
    {
        // Check whether the current visitor has a user ID in their MVC session.
        var userId = HttpContext.Session.GetInt32("UserId");

        // Visitors who are not logged in must sign in before continuing.
        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Read the role stored during the successful login process.
        var role = HttpContext.Session.GetString("Role");

        // Only participants may access participant-specific pages.
        if (!string.Equals(role, "Participant", StringComparison.Ordinal))
        {
            // Return HTTP 403 because this application uses session-based role checks.
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        // Display the participant landing page, which we will create next.
        return View();


    }



    // Displays events that participants can browse and enter.
    [HttpGet]
    public async Task<IActionResult> BrowseEvents()
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
            var events = await _apiClient.GetAvailableEventsAsync(
                HttpContext.Session);

            return View(events);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Unable to retrieve events: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Unable to retrieve available events. Please try again later.");
        }
    } // end of BrowseEvents method







    // Displays the details and available categories for a selected event.
    [HttpGet]
    public async Task<IActionResult> EventDetails(int id)
    {
        // Confirm that the user is logged in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only participants may access this page.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Participant", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            // Retrieve the selected event from the Part 2 API.
            var selectedEvent = await _apiClient.GetEventByIdAsync(
                id,
                HttpContext.Session);

            // Return a not-found response if the event does not exist.
            if (selectedEvent is null)
            {
                return NotFound();
            }

            // Retrieve the categories associated with this event.
            var categories = await _apiClient.GetCategoriesAsync(
                id,
                HttpContext.Session);

            // Combine the event and its categories into one ViewModel.
            var viewModel = new EventDetailsViewModel
            {
                Event = selectedEvent,
                Categories = categories
            };

            return View(viewModel);
        }
        catch (HttpRequestException ex)
        {
            // Log the API communication error for troubleshooting.
            Console.WriteLine($"Error retrieving event details: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Unable to retrieve event details at this time.");
        }
    }// end of EventDetails method


































































} // end of class
