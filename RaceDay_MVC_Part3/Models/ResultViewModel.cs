namespace RaceDay_MVC_Part3.Models;

public class ResultViewModel
{
    // Identifies the recorded result.
    public int ResultId { get; set; }

    // Connects the result to a participant's enrolment.
    public int EnrolmentId { get; set; }

    // Identifies the participant.
    public int ParticipantId { get; set; }

    // Participant and category information.
    public string ParticipantName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;

    // Recorded race performance.
    public int FinishingPosition { get; set; }
    public TimeOnly FinishTime { get; set; }
}