using MyMoneySaver.Models;

namespace MyMoneySaver.Services;

/// <summary>
/// Manages transactions with in-memory storage
/// </summary>
public class TransactionService
{
    private readonly List<Transaction> _transactions = new();
    private int _nextId = 1;

    /// <summary>
    /// Event fired when transactions change
    /// </summary>
    public event Action? OnTransactionsChanged;

    /// <summary>
    /// Gets all transactions
    /// </summary>
    public List<Transaction> GetAll() => _transactions;

    /// <summary>
    /// Gets transaction by ID
    /// </summary>
    public Transaction? GetById(int id) => _transactions.FirstOrDefault(t => t.Id == id);

    /// <summary>
    /// Gets filtered transactions
    /// </summary>
    /// <param name="categoryId">Filter by category (optional)</param>
    /// <param name="startDate">Filter by start date (optional)</param>
    /// <param name="endDate">Filter by end date (optional)</param>
    /// <param name="type">Filter by transaction type (optional)</param>
    public List<Transaction> GetFiltered(
        int? categoryId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        TransactionType? type = null)
    {
        var query = _transactions.AsEnumerable();

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (startDate.HasValue)
            query = query.Where(t => t.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(t => t.Date <= endDate.Value);

        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        return query.ToList();
    }

    /// <summary>
    /// Adds new transaction
    /// </summary>
    public void Add(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        transaction.Id = _nextId++;
        _transactions.Add(transaction);
        OnTransactionsChanged?.Invoke();
    }

    /// <summary>
    /// Updates existing transaction
    /// </summary>
    public void Update(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        var index = _transactions.FindIndex(t => t.Id == transaction.Id);
        if (index >= 0)
        {
            _transactions[index] = transaction;
            OnTransactionsChanged?.Invoke();
        }
    }

    /// <summary>
    /// Deletes transaction by ID
    /// </summary>
    public void Delete(int id)
    {
        var removed = _transactions.RemoveAll(t => t.Id == id);
        if (removed > 0)
        {
            OnTransactionsChanged?.Invoke();
        }
    }

    /// <summary>
    /// Calculates total balance (Income - Expenses)
    /// </summary>
    public decimal GetTotalBalance()
    {
        return _transactions.Sum(t =>
            t.Type == TransactionType.Income ? t.Amount : -t.Amount);
    }

    /// <summary>
    /// Calculates total income
    /// </summary>
    public decimal GetTotalIncome()
    {
        return _transactions
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount);
    }

    /// <summary>
    /// Calculates total expenses
    /// </summary>
    public decimal GetTotalExpenses()
    {
        return _transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);
    }

    /// <summary>
    /// Gets totals grouped by category
    /// </summary>
    public Dictionary<int, decimal> GetCategoryTotals()
    {
        return _transactions
            .GroupBy(t => t.CategoryId)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
    }
}
