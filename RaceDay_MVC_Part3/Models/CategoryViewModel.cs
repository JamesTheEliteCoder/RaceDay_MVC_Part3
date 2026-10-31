namespace RaceDay_MVC_Part3.Models;

public class CategoryViewModel
{
    // Identifies this category in the API database.
    public int CategoryId { get; set; }

    // Identifies the event this category belongs to.
    public int EventId { get; set; }

    // Identifies the route associated with this category.
    public int RouteId { get; set; }

    // The category's display name.
    public string Name { get; set; } = string.Empty;

    // Indicates whether this is an Age or Distance category.
    public string CategoryType { get; set; } = string.Empty;

    // Optional lower age limit for an Age category.
    public int? MinimumAge { get; set; }

    // Optional upper age limit for an Age category.
    public int? MaximumAge { get; set; }
}