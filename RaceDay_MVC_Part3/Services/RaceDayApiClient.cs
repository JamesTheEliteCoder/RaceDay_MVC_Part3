
using Microsoft.Extensions.Configuration;
using RaceDay_MVC_Part3.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using Microsoft.AspNetCore.Http;
namespace RaceDay_MVC_Part3.Services;

public class RaceDayApiClient
{// start of class
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public RaceDayApiClient(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    private HttpClient CreateClient()
    {
        var baseUrl = _configuration["ApiSettings:BaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                "The RaceDay API base URL has not been configured.");
        }

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(baseUrl);

        return client;
    }


    public async Task<ApiUserResponse?> LoginAsync(
        LoginRequest request,
        ISession mvcSession)
    {
        // Create an HTTP client configured with the API base address.
        using var client = CreateClient();

        // Send the credentials to the API's existing login endpoint.
        // The API remains responsible for verifying the password.
        using var response = await client.PostAsJsonAsync(
            "api/auth/login",
            request);

        // A 401 response means the credentials were not accepted.
        // Return null so the MVC controller can display a friendly message.
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return null;
        }

        // Surface unexpected API errors rather than treating them
        // as ordinary invalid-credential responses.
        response.EnsureSuccessStatusCode();

        // The API uses a session cookie to recognise authenticated users.
        // Capture the cookie issued by the successful login response.
        if (!response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            throw new InvalidOperationException(
                "The API login succeeded but no session cookie was returned.");
        }

        // Select the default ASP.NET Core session cookie.
        var sessionCookie = cookies.FirstOrDefault(cookie =>
            cookie.StartsWith(
                ".AspNetCore.Session=",
                StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrWhiteSpace(sessionCookie))
        {
            throw new InvalidOperationException(
                "The API session cookie could not be found.");
        }

        // Keep only the cookie name and value. Cookie attributes such as
        // Path and HttpOnly belong to the Set-Cookie response header.
        var cookiePair = sessionCookie.Split(';', 2)[0];

        // Store the API cookie in this visitor's server-side MVC session.
        // Never put this cookie into a view or browser-side JavaScript.
        mvcSession.SetString("RaceDayApiSessionCookie", cookiePair);

        // Read the safe user details returned by the API.
        var user = await response.Content.ReadFromJsonAsync<ApiUserResponse>();

        if (user is null)
        {
            // Remove the cookie if the API response was unexpectedly empty.
            mvcSession.Remove("RaceDayApiSessionCookie");

            throw new InvalidOperationException(
                "The API returned an empty login response.");
        }

        return user;
    }





}// end of class
