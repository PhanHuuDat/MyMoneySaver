namespace MyMoneySaver.Models;

/// <summary>
/// Transaction type classification
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Money going out (default)
    /// </summary>
    Expense = 0,

    /// <summary>
    /// Money coming in
    /// </summary>
    Income = 1
}
