
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RaceDay_MVC_Part3.Controllers;


public class ParticipantController : Controller
{
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
}
