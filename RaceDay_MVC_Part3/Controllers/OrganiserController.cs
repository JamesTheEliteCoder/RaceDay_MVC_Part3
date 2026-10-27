
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RaceDay_MVC_Part3.Controllers;

public class OrganiserController : Controller
{

    //Displays the organiser dashboard to authenticated organisers.
    [HttpGet]
    public IActionResult Dashboard()
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
            // Deny access instead of displaying organiser-only content.
            return Forbid();
        }

        // Display the dashboard view, which we will create next.
        return View();
    }
}
