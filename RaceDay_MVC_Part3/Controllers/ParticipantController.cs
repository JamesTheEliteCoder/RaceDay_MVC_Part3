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

            // TEMPORARY DIAGNOSTIC: Check whether banner URLs reach MVC.
            foreach (var raceEvent in events)
            {
                Console.WriteLine(
                    $"Event: {raceEvent.Name} | " +
                    $"ID: {raceEvent.EventId} | " +
                    $"Banner URL: {raceEvent.BannerImageUrl}");
            }

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







    // Displays all events the signed-in participant has enrolled in.
    [HttpGet]
    public async Task<IActionResult> MyEvents()
    {
        // Confirm that the participant is logged in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only participants may access their personal enrolments.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Participant", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            // Retrieve the participant's enrolments through the Part 2 API.
            var enrolments = await _apiClient.GetMyEnrolmentsAsync(
                HttpContext.Session);

            return View(enrolments);
        }
        catch (HttpRequestException ex)
        {
            // Log API communication errors for troubleshooting.
            Console.WriteLine($"Error retrieving participant enrolments: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Unable to retrieve your events at this time.");
        }
    } // end of MyEvents method










    // Submits a participant's enrolment for the selected event category.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enrol(int eventId, int categoryId)
    {
        // Confirm that the participant is logged in.
        var userId = HttpContext.Session.GetInt32("UserId");



        /*temp lines
        // Temporary diagnostic checks for the MVC session.
        var sessionUserId = HttpContext.Session.GetInt32("UserId");


        // Temporary diagnostic: display the names of all MVC session keys.
        foreach (var key in HttpContext.Session.Keys)
        {
            Console.WriteLine($"MVC Session Key: {key}");
        }

        var sessionRole = HttpContext.Session.GetString("Role");

        Console.WriteLine($"MVC Session UserId: {sessionUserId}");
        Console.WriteLine($"MVC Session Role: {sessionRole}");

        */


        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only participants may enrol in events.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Participant", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        // Ensure valid event and category IDs were submitted.
        if (eventId <= 0 || categoryId <= 0)
        {
            TempData["ErrorMessage"] = "Invalid event or category selection.";
            return RedirectToAction(nameof(BrowseEvents));
        }

        try
        {
            // Forward the enrolment request to the Part 2 API.
            var enrolmentSuccessful = await _apiClient.EnrolInEventAsync(
                eventId,
                categoryId,
                HttpContext.Session);

            if (enrolmentSuccessful)
            {
                TempData["SuccessMessage"] =
                    "Your enrolment was successful. Good luck with your race!";
            }
            else
            {
                TempData["ErrorMessage"] =
                    "We couldn't complete your enrolment. You may already be enrolled in this category, or the selection may be invalid.";
            }
        }
        catch (HttpRequestException ex)
        {
            // Log communication errors without exposing technical details to the participant.
            Console.WriteLine($"Error submitting enrolment: {ex.Message}");

            TempData["ErrorMessage"] =
                "We're unable to process your enrolment right now. Please try again later.";
        }

        // Return the participant to the event they attempted to join.
        return RedirectToAction(nameof(EventDetails), new { id = eventId });
    }// end of Enrol method






    // Displays all recorded race results belonging to the signed-in participant.
    [HttpGet]
    public async Task<IActionResult> MyResults()
    {
        // Confirm that the participant is logged in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only participants may access their personal race results.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Participant", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            // Retrieve the participant's results through the Part 2 API.
            var results = await _apiClient.GetMyResultsAsync(
                HttpContext.Session);

            return View(results);
        }
        catch (HttpRequestException ex)
        {
            // Log API communication errors for troubleshooting.
            Console.WriteLine($"Error retrieving participant results: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "Unable to retrieve your race results at this time.");
        }
    } // end of MyResults method





























































} // end of class
