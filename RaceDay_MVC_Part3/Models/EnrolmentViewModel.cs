namespace RaceDay_MVC_Part3.Models;

public class EnrolmentViewModel
{
    // Identifies the enrolment record.
    public int EnrolmentId { get; set; }

    // Participant information supplied by the API.
    public int ParticipantId { get; set; }
    public string ParticipantName { get; set; } = string.Empty;
    public string ParticipantEmail { get; set; } = string.Empty;

    // Event and selected category information.
    public int EventId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    // Enrolment date and current status.
    public DateTime EnrolmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
}