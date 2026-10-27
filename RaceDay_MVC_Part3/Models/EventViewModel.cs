namespace RaceDay_MVC_Part3.Models;


public class EventViewModel
{
    // Unique identifier assigned to the event by the API.
    public int EventId { get; set; }

    // ID of the organiser who owns this event.
    public int OrganiserId { get; set; }

    // Basic information displayed on the organiser dashboard.
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Date and location information for the event.
    public DateOnly EventDate { get; set; }
    public string Venue { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;

    // Distance and activity type help participants understand the event.
    public decimal DistanceKm { get; set; }
    public string EventType { get; set; } = string.Empty;

    // Timestamp recorded when the event was created.
    public DateTime CreatedOn { get; set; }
}