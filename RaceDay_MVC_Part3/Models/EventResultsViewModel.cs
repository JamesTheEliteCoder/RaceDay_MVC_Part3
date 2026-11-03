namespace RaceDay_MVC_Part3.Models;

public class EventResultsViewModel
{
    // Identifies the event being managed.
    public int EventId { get; set; }

    public string EventName { get; set; } = string.Empty;

    // Contains every participant enrolled in the event.
    public List<EnrolmentViewModel> Enrolments { get; set; } = new();

    // Contains results that have already been recorded.
    public List<ResultViewModel> Results { get; set; } = new();
}