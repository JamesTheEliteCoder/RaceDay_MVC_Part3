namespace RaceDay_MVC_Part3.Models;

public class EventEnrolmentsViewModel
{
    // Identifies the event whose enrolments are being displayed.
    public int EventId { get; set; }

    public string EventName { get; set; } = string.Empty;

    // Contains all participants enrolled in this event.
    public List<EnrolmentViewModel> Enrolments { get; set; } = new();
}