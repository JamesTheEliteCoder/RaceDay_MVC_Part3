namespace RaceDay_MVC_Part3.Models;

public class EventManagementViewModel
{
    // Identifies the event being managed.
    public int EventId { get; set; }

    // Displays the event name on the management page.
    public string EventName { get; set; } = string.Empty;

    // Holds all routes associated with this event.
    public List<RouteViewModel> Routes { get; set; } = new();

    // Holds all categories associated with this event.
    public List<CategoryViewModel> Categories { get; set; } = new();
}