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
    }
}