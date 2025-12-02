using System.ComponentModel.DataAnnotations;

namespace MyMoneySaver.Models;

/// <summary>
/// Financial transaction record
/// </summary>
public class Transaction
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Transaction amount (positive value)
    /// </summary>
    [Required]
    [Range(0.01, 1000000, ErrorMessage = "Amount must be between 0.01 and 1,000,000")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Associated category ID
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }

    /// <summary>
    /// Transaction description/note
    /// </summary>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Transaction date
    /// </summary>
    [Required]
    public DateTime Date { get; set; } = DateTime.Today;

    /// <summary>
    /// Transaction type (Income or Expense)
    /// </summary>
    [Required]
    public TransactionType Type { get; set; } = TransactionType.Expense;

    /// <summary>
    /// Navigation property to Category (optional, for display purposes)
    /// </summary>
    public Category? Category { get; set; }
}
