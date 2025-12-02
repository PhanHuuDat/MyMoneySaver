using System.ComponentModel.DataAnnotations;

namespace MyMoneySaver.Models;

/// <summary>
/// Category for transaction classification
/// </summary>
public class Category
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Category display name
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Material Design icon name (e.g., "restaurant", "directions_car")
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Icon { get; set; } = "category";

    /// <summary>
    /// Hex color code (e.g., "#ff9800")
    /// </summary>
    [Required]
    [StringLength(7, MinimumLength = 7)]
    [RegularExpression(@"^#[0-9a-fA-F]{6}$", ErrorMessage = "Color must be valid hex format (#RRGGBB)")]
    public string Color { get; set; } = "#1976d2";
}
