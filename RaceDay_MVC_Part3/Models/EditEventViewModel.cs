
using System.ComponentModel.DataAnnotations;

namespace RaceDay_MVC_Part3.Models;

/// <summary>
/// Holds the event details an organiser can edit.
/// Validation helps catch invalid input before it reaches the API.
/// </summary>
public class EditEventViewModel
{
    // The ID identifies which existing event is being edited.
    public int EventId { get; set; }

    [Required(ErrorMessage = "Please enter an event name.")]
    [StringLength(30, ErrorMessage = "The event name cannot exceed 30 characters.")]
    [Display(Name = "Event name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter an event description.")]
    [StringLength(500, ErrorMessage = "The description cannot exceed 500 characters.")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select the event date.")]
    [DataType(DataType.Date)]
    [Display(Name = "Event date")]
    public DateOnly EventDate { get; set; }

    [Required(ErrorMessage = "Please enter the venue.")]
    [StringLength(100, ErrorMessage = "The venue cannot exceed 100 characters.")]
    public string Venue { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the city.")]
    [StringLength(50, ErrorMessage = "The city cannot exceed 50 characters.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the province.")]
    [StringLength(50, ErrorMessage = "The province cannot exceed 50 characters.")]
    public string Province { get; set; } = string.Empty;

    [Range(0.01, 9999.99,
        ErrorMessage = "Distance must be greater than zero and no more than 9999.99 km.")]
    [Display(Name = "Distance (km)")]
    public decimal DistanceKm { get; set; }

    [Required(ErrorMessage = "Please select an event type.")]
    [RegularExpression("^(Run|Walk|Cycle)$",
        ErrorMessage = "Event type must be Run, Walk, or Cycle.")]
    [Display(Name = "Event type")]
    public string EventType { get; set; } = string.Empty;
}
