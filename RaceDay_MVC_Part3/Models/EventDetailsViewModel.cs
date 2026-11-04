namespace RaceDay_MVC_Part3.Models;

public class EventDetailsViewModel
{
    // Holds the main details of the selected event.
    public EventViewModel Event { get; set; } = new();

    // Holds the categories participants can choose from.
    public List<CategoryViewModel> Categories { get; set; } = new();
}