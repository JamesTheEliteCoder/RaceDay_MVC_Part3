
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

        // Only organisers may manage event routes and categories.
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

    // Displays the routes and categories belonging to an organiser's event.
    [HttpGet]
    public async Task<IActionResult> ManageEvent(int id)
    {
        // Ensure that the visitor is logged in as an organiser.
        if (HttpContext.Session.GetInt32("UserId") == null ||
            HttpContext.Session.GetString("Role") != "Organiser")
        {
            return RedirectToAction("Login", "Account");
        }

        try
        {
            // Retrieve the events owned by the current organiser.
            var events = await _apiClient.GetMyEventsAsync(HttpContext.Session);

            // Only allow management of an event belonging to this organiser.
            var selectedEvent = events.FirstOrDefault(raceEvent =>
                raceEvent.EventId == id);

            if (selectedEvent == null)
            {
                return NotFound();
            }

            // Retrieve the routes and categories for the selected event.
            var routes = await _apiClient.GetRoutesAsync(
                id, HttpContext.Session);

            var categories = await _apiClient.GetCategoriesAsync(
                id, HttpContext.Session);

            // Combine the information into one page model.
            var viewModel = new EventManagementViewModel
            {
                EventId = selectedEvent.EventId,
                EventName = selectedEvent.Name,
                Routes = routes,
                Categories = categories
            };

            return View(viewModel);
        }
        catch (HttpRequestException ex)
        {
            // Log the API connectivity problem for troubleshooting.
            Console.WriteLine(
                $"Unable to load event management data: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "The RaceDay API could not be reached. Please try again later.");
        }
    } // End of ManageEvent method




    // Displays all participants enrolled in an event managed by the organiser.
    [HttpGet]
    public async Task<IActionResult> ViewEnrolments(int id)
    {
        // Ensure the user is signed in as an organiser.
        var userId = HttpContext.Session.GetInt32("UserId");
        var role = HttpContext.Session.GetString("Role");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (role != "Organiser")
        {
            return Forbid();
        }

        try
        {
            // Retrieve the organiser's events to verify ownership.
            var events = await _apiClient.GetMyEventsAsync(HttpContext.Session);

            var selectedEvent = events.FirstOrDefault(raceEvent =>
                raceEvent.EventId == id);

            if (selectedEvent is null)
            {
                return NotFound();
            }

            // Retrieve the enrolments for the selected event.
            var enrolments = await _apiClient.GetEventEnrolmentsAsync(
                id,
                HttpContext.Session);

            var viewModel = new EventEnrolmentsViewModel
            {
                EventId = selectedEvent.EventId,
                EventName = selectedEvent.Name,
                Enrolments = enrolments
            };

            return View(viewModel);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error retrieving event enrolments: {ex.Message}");

            return StatusCode(502,
                "Unable to retrieve enrolment information from the API.");
        }
    } // End of ViewEnrolments method



    // Displays the form used to add a route to an existing event.
    [HttpGet]
    public async Task<IActionResult> CreateRoute(int eventId)
    {
        // Require the visitor to be signed in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only organisers may create event routes.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Organiser", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            // Retrieve events belonging to the signed-in organiser.
            var myEvents = await _apiClient.GetMyEventsAsync(HttpContext.Session);

            // Confirm that the requested event belongs to this organiser.
            var selectedEvent = myEvents.FirstOrDefault(
                raceEvent => raceEvent.EventId == eventId);

            if (selectedEvent is null)
            {
                return NotFound();
            }

            // Prepare the form with the selected event ID.
            var model = new CreateRouteViewModel
            {
                EventId = selectedEvent.EventId
            };

            return View(model);
        }
        catch (HttpRequestException ex)
        {
            // Log API connectivity problems for troubleshooting.
            Console.WriteLine(
                $"Unable to load route creation form: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "The RaceDay API could not be reached. Please try again later.");
        }
    } // end of GET action





    // Validates and submits a new route to the Part 2 API.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoute(CreateRouteViewModel model)
    {
        // Require the visitor to be signed in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only organisers may create event routes.
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
            // Submit the route using the authenticated API client.
            using var response = await _apiClient.CreateRouteAsync(
                model,
                HttpContext.Session);

            // Return to event management after successful creation.
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    nameof(ManageEvent),
                    new { id = model.EventId });
            }

            // Handle authentication and permission errors.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Account");
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "You are not authorised to add routes to this event.");
            }

            // Preserve the form and show an error if the API rejects the route.
            ModelState.AddModelError(
                string.Empty,
                "The route could not be created. Please check the details and try again.");

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
    } // end of POST action



    // Displays the form used to add a category to an existing event.
    [HttpGet]
    public async Task<IActionResult> CreateCategory(int eventId)
    {
        // Require the visitor to be signed in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only organisers may create event categories.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Organiser", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            // Retrieve events belonging to the signed-in organiser.
            var myEvents = await _apiClient.GetMyEventsAsync(HttpContext.Session);

            // Confirm that the requested event belongs to this organiser.
            var selectedEvent = myEvents.FirstOrDefault(
                raceEvent => raceEvent.EventId == eventId);

            if (selectedEvent is null)
            {
                return NotFound();
            }

            // Retrieve the routes available for this event.
            var routes = await _apiClient.GetRoutesAsync(
                eventId, HttpContext.Session);

            // Prepare the category form and its route dropdown options.
            var model = new CreateCategoryViewModel
            {
                EventId = selectedEvent.EventId,
                AvailableRoutes = routes
            };

            return View(model);
        }
        catch (HttpRequestException ex)
        {
            // Log API connectivity problems for troubleshooting.
            Console.WriteLine(
                $"Unable to load category creation form: {ex.Message}");

            return StatusCode(
                StatusCodes.Status502BadGateway,
                "The RaceDay API could not be reached. Please try again later.");
        }
    } // end of GET action (CreateCategory Method)







    // Validates and submits a new category to the Part 2 API.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(
        CreateCategoryViewModel model)
    {
        // Require the visitor to be signed in.
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Only organisers may create event categories.
        var role = HttpContext.Session.GetString("Role");

        if (!string.Equals(role, "Organiser", StringComparison.Ordinal))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        // Reject invalid form data before calling the API.
        if (!ModelState.IsValid)
        {
            await ReloadAvailableRoutesAsync(model);
            return View(model);
        }

        try
        {
            // Submit the category using the authenticated API client.
            using var response = await _apiClient.CreateCategoryAsync(
                model,
                HttpContext.Session);

            // Return to event management after successful creation.
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    nameof(ManageEvent),
                    new { id = model.EventId });
            }

            // Handle authentication and permission errors.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Account");
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    "You are not authorised to add categories to this event.");
            }

            // Display a useful message if the API rejects the category.
            ModelState.AddModelError(
                string.Empty,
                "The category could not be created. Please check the details and try again.");

            await ReloadAvailableRoutesAsync(model);
            return View(model);
        }
        catch (HttpRequestException)
        {
            // Preserve the entered details if the API cannot be reached.
            ModelState.AddModelError(
                string.Empty,
                "The RaceDay API could not be reached. Please try again later.");

            await ReloadAvailableRoutesAsync(model);
            return View(model);
        }
    } // end of POST action (CreateCategory Method)





    // Reloads the event's routes when the category form must be redisplayed.
    private async Task ReloadAvailableRoutesAsync(
        CreateCategoryViewModel model)
    {
        model.AvailableRoutes = await _apiClient.GetRoutesAsync(
            model.EventId,
            HttpContext.Session);
    } // end of ReloadAvailableRoutesAsync helper method


} // End of OrganiserController class







