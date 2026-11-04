namespace RaceDay_MVC_Part3.Models;

public class ParticipantEnrolmentViewModel
{
    public int EnrolmentId { get; set; }

    public int EventId { get; set; }

    public string EventName { get; set; } = string.Empty;

    public DateOnly EventDate { get; set; }

    public string Venue { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public DateTime EnrolmentDate { get; set; }

    public string Status { get; set; } = string.Empty;
}