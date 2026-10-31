namespace RaceDay_MVC_Part3.Models;

public class RouteViewModel
{
    // Identifies this route in the API database.
    public int RouteId { get; set; }

    // Identifies the event this route belongs to.
    public int EventId { get; set; }

    // The route's display name.
    public string RouteName { get; set; } = string.Empty;

    // Describes the areas covered by the route.
    public string AreasCovered { get; set; } = string.Empty;

    // Total route distance, measured in kilometres.
    public decimal DistanceKm { get; set; }

    // Where participants begin the route.
    public string StartLocation { get; set; } = string.Empty;

    // Where participants finish the route.
    public string FinishLocation { get; set; } = string.Empty;

    // Optional additional information about the route.
    public string? Description { get; set; }

    // Optional link to a route map.
    public string? MapUrl { get; set; }
}