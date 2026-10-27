
using Microsoft.AspNetCore.Mvc;
using RaceDay_MVC_Part3.Models;
using RaceDay_MVC_Part3.Services;

namespace RaceDay_MVC_Part3.Controllers;

/// <summary>
/// Handles account-related pages and login requests.
/// The Part 2 API remains responsible for verifying user credentials.
/// </summary>
public class AccountController : Controller
{
    private readonly RaceDayApiClient _apiClient;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        RaceDayApiClient apiClient,
        ILogger<AccountController> logger)
    {
        // Reuse the service that communicates with the Part 2 API.
        _apiClient = apiClient;

        // Log unexpected errors without logging passwords or session cookies.
        _logger = logger;
    }

    /// <summary>
    /// Displays the login form.
    /// </summary>
    [HttpGet]
    public IActionResult Login()
    {
        // Supply an empty model so the form can display its input fields.
        return View(new LoginViewModel());
    }

    /// <summary>
    /// Submits login credentials to the Part 2 API.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Reject invalid form input before contacting the API.
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Convert the MVC form model into the API's login request model.
            var request = new LoginRequest
            {
                Email = model.Email.Trim(),
                Password = model.Password
            };

            // Ask the API to verify the credentials and establish its session.
            var user = await _apiClient.LoginAsync(
                request,
                HttpContext.Session);

            // A null result means the API rejected the credentials.
            if (user is null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "The email address or password is incorrect.");

                return View(model);
            }

            // Save only safe user details in the MVC session.
            // The API session cookie is stored separately by LoginAsync.
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("Role", user.Role);

            // Send each role to its own landing page.
            if (string.Equals(
                user.Role,
                "Organiser",
                StringComparison.Ordinal))
            {
                return RedirectToAction("Dashboard", "Organiser");
            }

            if (string.Equals(
                user.Role,
                "Participant",
                StringComparison.Ordinal))
            {
                return RedirectToAction("Index", "Participant");
            }

            // Fail safely if the API returns an unexpected role value.
            HttpContext.Session.Clear();

            ModelState.AddModelError(
                string.Empty,
                "Your account has an unrecognised role. Please contact support.");

            return View(model);
        }
        catch (HttpRequestException ex)
        {
            // Record the technical issue without exposing it to the user.
            _logger.LogError(ex, "The RaceDay API could not process a login request.");

            ModelState.AddModelError(
                string.Empty,
                "We couldn't connect to the service. Please try again shortly.");

            return View(model);
        }
        catch (InvalidOperationException ex)
        {
            // Configuration or an unexpected API response may cause this error.
            _logger.LogError(ex, "The RaceDay API returned an unexpected login response.");

            ModelState.AddModelError(
                string.Empty,
                "We couldn't complete your login. Please try again shortly.");

            return View(model);
        }
    }




    /// Signs the visitor out of the MVC application and requests that the Part 2 API end its session as well.
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        try
        {
            // Ask the API to end the session associated with this visitor.
            await _apiClient.LogoutAsync(HttpContext.Session);
        }
        catch (HttpRequestException ex)
        {
            // Log connectivity failures without exposing session details.
            _logger.LogWarning( ex,"The API could not complete the logout request.");
        }
        catch (InvalidOperationException ex)
        {
            // Log unexpected API responses while continuing local logout.
            _logger.LogWarning(ex, "The API returned an unexpected logout response.");
        }
        finally
        {
            // Always clear local authentication details, even if the API fails.
            HttpContext.Session.Clear();
        }

        // Return the signed-out visitor to the login page.
        return RedirectToAction("Login", "Account");
    }







}//end of class
