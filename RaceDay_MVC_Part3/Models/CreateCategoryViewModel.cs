using System.ComponentModel.DataAnnotations;

namespace RaceDay_MVC_Part3.Models;

public class CreateCategoryViewModel
{
    // Identifies the event to which this category will be added.
    public int EventId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        "^(Age|Distance)$",
        ErrorMessage = "Category type must be Age or Distance.")]
    public string CategoryType { get; set; } = string.Empty;

    // The organiser must select an existing route for this category.
    [Range(1, int.MaxValue)]
    public int RouteId { get; set; }

    // These fields are only required for Age categories.
    [Range(0, int.MaxValue)]
    public int? MinimumAge { get; set; }

    [Range(0, int.MaxValue)]
    public int? MaximumAge { get; set; }

    // Routes are loaded by the controller to populate the form's dropdown.
    public List<RouteViewModel> AvailableRoutes { get; set; } = new();
}