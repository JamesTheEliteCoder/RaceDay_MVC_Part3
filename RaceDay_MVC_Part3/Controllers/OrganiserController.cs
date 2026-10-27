using RaceDay_MVC_Part3.Models;
using RaceDay_MVC_Part3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    // Displays the organiser dashboard to authenticated organisers.
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        // Check whether a user ID exists in the current visitor's MVC session.
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
            // Deny access with HTTP 403 because we use session-based role checks.
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            // Retrieve only the events belonging to this organiser.
            List<EventViewModel> events =
                await _apiClient.GetMyEventsAsync(HttpContext.Session);

            // Pass the events to the dashboard view for display.
            return View(events);
        }
        catch (HttpRequestException ex)
        {
            // Log API connectivity failures without exposing internal details.
            // A logger will be added in a later step.
            Console.WriteLine($"Failed to retrieve organiser events: {ex.Message}");

            // Return a gateway error rather than displaying a misleading
            // empty event list when the API request fails.
            return StatusCode(
                StatusCodes.Status502BadGateway,
                "The RaceDay API could not be reached. Please try again later.");
        }
    } //end of Dashboard method


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

        // Display a form with the default values from the view model.
        return View(new CreateEventViewModel());
    }

    /// <summary>
    /// Validates and submits a new event to the Part 2 API.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(
        CreateEventViewModel model)
    {
        // Require the visitor to be signed in before processing the form.
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

        // Redisplay the form when MVC validation fails.
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
                // Return to the dashboard, which will retrieve the updated list.
                return RedirectToAction(nameof(Dashboard));
            }

            // Preserve the entered values and display a helpful error.
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your session has expired. Please sign in again.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
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
            // Keep the entered details available if the API cannot be reached.
            ModelState.AddModelError(
                string.Empty,
                "The RaceDay API could not be reached. Please try again later.");

            return View(model);
        }
    }



}//end of class