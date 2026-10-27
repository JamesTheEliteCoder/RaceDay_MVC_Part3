using Microsoft.AspNetCore.Mvc;
using RaceDay_MVC_Part3.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;


namespace RaceDay_MVC_Part3.Controllers
{
    public class HomeController : Controller
    {

        /// <summary>
        /// Directs visitors to the appropriate starting page for their session.
        /// </summary>
        public IActionResult Index()
        {
            // Read the user's ID and role from the MVC session.
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            // Visitors without a login session must sign in first.
            if (userId is null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Send organisers to their dedicated dashboard.
            if (string.Equals(role, "Organiser", StringComparison.Ordinal))
            {
                return RedirectToAction("Dashboard", "Organiser");
            }

            // Send participants to their dedicated landing page.
            if (string.Equals(role, "Participant", StringComparison.Ordinal))
            {
                return RedirectToAction("Index", "Participant");
            }

            // Clear invalid session details and ask the visitor to sign in again.
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
