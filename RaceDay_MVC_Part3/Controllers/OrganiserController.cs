
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDay_MVC_Part3.Models;
using RaceDay_MVC_Part3.Services;
using System.Net;

namespace RaceDay_MVC_Part3.Controllers;

public class OrganiserController : Controller
{
    // Provides methods for communicating with the Part 2 API.
    private readonly RaceDayApiClient _apiClient;

    // Receive the API client through ASP.NET Core dependency injection.
    public OrganiserController(RaceDayApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Displays the organiser dashboard and retrieves the organiser's events.
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        // Check whether a user ID exists in the current MVC session.
        var userId = HttpContext.Session.GetInt32("UserId");

        // Visitors who have not signed in must authenticate first.
        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only users with the Organiser role may access this dashboard.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Organiser", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            // Retrieve only events belonging to the signed-in organiser.
            List<EventViewModel> events =
                await _apiClient.GetMyEventsAsync(HttpContext.Session);

            return View(events);
        }
        catch (HttpRequestException ex)
        {
            // Log the connectivity problem without exposing it to the visitor.
            Console.WriteLine(
                $"Failed to retrieve organiser events: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "The RaceDay API could not be reached. Please try again later.");
        }
    }

    // Displays the form used to create a new event.
    [HttpGet]
    public IActionResult CreateEvent()
    {
        // Require the visitor to be signed in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only organisers may create events.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Organiser", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        // Provide an initial model for the empty form.
        return View(new CreateEventViewModel());
    }

    // Validates and submits a new event to the Part 2 API.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(
        CreateEventViewModel model)
    {
        // Require the visitor to be signed in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only organisers may create events.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Organiser", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        // Redisplay the form when validation fails.
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Send the event details through the authenticated API client.
            using var response = await _apiClient.CreateEventAsync(
                model,
                HttpContext.Session);

            // A successful API creation returns HTTP 201 Created.
            if (response.IsSuccessStatusCode)
            {
                // The dashboard will retrieve the updated event list.
                return RedirectToAction(nameof(Dashboard));
            }

            // Handle authentication and other API errors.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your session has expired. Please sign in again.");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "You are not authorised to create events.");
            }
            else
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The event could not be created. Please check the details and try again.");
            }

            return View(model);
        }
        catch (HttpRequestException)
        {
            // Preserve the entered details if the API cannot be reached.
            ModelState.AddModelError(
                string.Empty,
                "The RaceDay API could not be reached. Please try again later.");

            return View(model);
        }
    }

    // Displays an existing event's details in the editing form.
    [HttpGet]
    public async Task<IActionResult> EditEvent(int id)
    {
        // Ensure the visitor is signed in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only organisers may edit events.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Organiser", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }


        try
        {
            // Retrieve only the events belonging to the signed-in organiser.
            var myEvents = await _apiClient.GetMyEventsAsync(HttpContext.Session);

            // Do not allow an organiser to open another organiser's event.
            var raceEvent = myEvents.FirstOrDefault(
                eventItem => eventItem.EventId == id);

            if (raceEvent is null)
            {
                // Avoid revealing whether another organiser's event exists.
                return NotFound();
            }

            // Map the API event to the model used by the editing form.
            var model = new EditEventViewModel
            {
                EventId = raceEvent.EventId,
                Name = raceEvent.Name,
                Description = raceEvent.Description,
                EventDate = raceEvent.EventDate,
                Venue = raceEvent.Venue,
                City = raceEvent.City,
                Province = raceEvent.Province,
                DistanceKm = raceEvent.DistanceKm,
                EventType = raceEvent.EventType
            };

            return View(model);
        }
        catch (HttpRequestException)
        {
            // Report API connectivity problems without exposing internal details.
            return StatusCode(
                StatusCodes.Status502BadGateway,
                "The RaceDay API could not be reached. Please try again later.");
        }
    } // End of EditEvent method


    // Validates and submits changes to an existing event.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEvent(EditEventViewModel model)
    {
        // Require the visitor to be signed in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only organisers may edit events.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Organiser", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        // Reject invalid form data before calling the API.
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Send the updated details to the API.
            // The API independently verifies event ownership.
            using var response = await _apiClient.UpdateEventAsync(
                model.EventId,
                model,
                HttpContext.Session);

            // Return to the dashboard after a successful update.
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Dashboard));
            }

            // Handle common API errors without losing the submitted form.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Account");
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "You are not authorised to edit this event.");
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            ModelState.AddModelError(
                string.Empty,
                "The event could not be updated. Please check the details and try again.");

            return View(model);
        }
        catch (HttpRequestException)
        {
            // Keep the entered details available if the API cannot be reached.
            ModelState.AddModelError(
                string.Empty,
                "The RaceDay API could not be reached. Please try again later.");

            return View(model);
        }
    }




} // End of OrganiserController class







