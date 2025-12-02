# Phase 02: Services

## Context Links

- **Parent Plan:** [plan.md](plan.md)
- **Dependencies:** Phase 01 (Data Models)
- **References:**
  - [Brainstorm Report](../reports/brainstorm-251201-client-side-money-tracker.md)
  - [Code Standards](../../docs/code-standards.md)
  - [System Architecture](../../docs/system-architecture.md)

## Overview

**Date:** 2025-12-01
**Description:** Create scoped services for in-memory data management with CRUD operations and reactive events
**Priority:** P0 (Foundation)
**Implementation Status:** 🔵 Not Started
**Review Status:** Pending

## Key Insights

- Services scoped to session (per-user isolation)
- In-memory List<T> storage (resets on app restart)
- Event-driven architecture (OnChanged events)
- Auto-incrementing IDs (_nextId field)
- CategoryService seeds default categories
- TransactionService provides filtering and summary methods
- No async needed (in-memory operations fast)

## Requirements

### Functional
- CategoryService: CRUD + seed 6 default categories
- TransactionService: CRUD + filtering + summary calculations
- Event notifications for reactive UI updates
- Auto-increment ID generation
- Support multiple concurrent sessions (scoped lifetime)

### Non-Functional
- File size < 200 lines per file
- XML comments on public methods
- Event pattern for UI reactivity
- YAGNI: Only required methods
- KISS: Simple in-memory implementation
- DRY: No code duplication

## Architecture

### Service Layer Pattern

```
Component (UI)
  ↓ Inject
Service (Scoped)
  ↓ Manages
In-Memory List<T>
  ↓ Fires
OnChanged Event
  ↓ Triggers
Component Re-render
```

### Event-Driven Updates

```
User Action → Service Method → Modify List → Fire Event → Component Subscribes → StateHasChanged() → UI Updates
```

### Scoped Lifetime

```
Browser Session Start
  ↓
DI Container creates service instance
  ↓
Service initializes (CategoryService seeds data)
  ↓
User interacts with UI
  ↓
Session ends / Page refresh
  ↓
Service disposed, data lost
```

## Related Code Files

### Files to Create
1. `MyMoneySaver/MyMoneySaver/Services/CategoryService.cs` (~120 lines)
2. `MyMoneySaver/MyMoneySaver/Services/TransactionService.cs` (~150 lines)

### Dependencies
- `MyMoneySaver/MyMoneySaver/Models/Category.cs`
- `MyMoneySaver/MyMoneySaver/Models/Transaction.cs`
- `MyMoneySaver/MyMoneySaver/Models/TransactionType.cs`

## Implementation Steps

### Step 1: Create Services Folder
```bash
cd MyMoneySaver/MyMoneySaver
mkdir Services
```

### Step 2: Create CategoryService.cs (~120 lines)

**File:** `Services/CategoryService.cs`

```csharp
using MyMoneySaver.Models;

namespace MyMoneySaver.Services;

/// <summary>
/// Manages categories with in-memory storage
/// </summary>
public class CategoryService
{
    private readonly List<Category> _categories = new();
    private int _nextId = 1;

    /// <summary>
    /// Event fired when categories change
    /// </summary>
    public event Action? OnCategoriesChanged;

    /// <summary>
    /// Initializes service with default categories
    /// </summary>
    public CategoryService()
    {
        SeedDefaultCategories();
    }

    /// <summary>
    /// Seeds default category data
    /// </summary>
    private void SeedDefaultCategories()
    {
        _categories.AddRange(new[]
        {
            new Category
            {
                Id = _nextId++,
                Name = "Food",
                Icon = "restaurant",
                Color = "#ff9800"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Transport",
                Icon = "directions_car",
                Color = "#2196f3"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Entertainment",
                Icon = "movie",
                Color = "#e91e63"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Shopping",
                Icon = "shopping_cart",
                Color = "#9c27b0"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Bills",
                Icon = "receipt",
                Color = "#f44336"
            },
            new Category
            {
                Id = _nextId++,
                Name = "Other",
                Icon = "category",
                Color = "#607d8b"
            }
        });
    }

    /// <summary>
    /// Gets all categories
    /// </summary>
    public List<Category> GetAll() => _categories;

    /// <summary>
    /// Gets category by ID
    /// </summary>
    public Category? GetById(int id) => _categories.FirstOrDefault(c => c.Id == id);

    /// <summary>
    /// Adds new category
    /// </summary>
    public void Add(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        category.Id = _nextId++;
        _categories.Add(category);
        OnCategoriesChanged?.Invoke();
    }

    /// <summary>
    /// Updates existing category
    /// </summary>
    public void Update(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        var index = _categories.FindIndex(c => c.Id == category.Id);
        if (index >= 0)
        {
            _categories[index] = category;
            OnCategoriesChanged?.Invoke();
        }
    }

    /// <summary>
    /// Deletes category by ID
    /// </summary>
    public void Delete(int id)
    {
        var removed = _categories.RemoveAll(c => c.Id == id);
        if (removed > 0)
        {
            OnCategoriesChanged?.Invoke();
        }
    }
}
```

**Key Points:**
- Constructor seeds 6 default categories
- _nextId auto-increments for unique IDs
- OnCategoriesChanged event for reactive UI
- ArgumentNullException for null checks
- Only fire event if data actually changed (Delete checks removed > 0)

### Step 3: Create TransactionService.cs (~150 lines)

**File:** `Services/TransactionService.cs`

```csharp
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
```

**Key Points:**
- GetFiltered supports optional parameters (all nullable)
- LINQ chaining for flexible filtering
- GetTotalBalance: Income positive, Expense negative
- GetTotalIncome/GetTotalExpenses: Separate calculations
- GetCategoryTotals: Dictionary for easy lookup
- OnTransactionsChanged event for reactive UI

## Todo List

- [ ] Create Services folder in MyMoneySaver/MyMoneySaver
- [ ] Create CategoryService.cs with seed data
- [ ] Create TransactionService.cs with filtering
- [ ] Verify both files compile without errors
- [ ] Test seed data creates 6 categories
- [ ] Verify auto-increment IDs work correctly
- [ ] Check file sizes (should be under 200 lines)
- [ ] Verify XML comments on public methods

## Success Criteria

### Compilation
- [ ] Project builds without errors
- [ ] No compiler warnings about nullability
- [ ] Services reference Models namespace correctly

### Code Quality
- [x] CategoryService.cs < 130 lines ✓
- [x] TransactionService.cs < 160 lines ✓
- [x] XML comments on all public methods ✓
- [x] Event pattern implemented correctly ✓
- [x] ArgumentNullException checks added ✓

### Functionality
- [ ] CategoryService seeds 6 default categories
- [ ] Auto-increment IDs start at 1
- [ ] GetAll returns all items
- [ ] GetById returns correct item or null
- [ ] Add/Update/Delete fire OnChanged events
- [ ] GetFiltered handles all parameter combinations
- [ ] GetTotalBalance calculates correctly
- [ ] GetCategoryTotals groups by category

### Standards Compliance
- [x] PascalCase for class/method names ✓
- [x] camelCase with underscore for private fields ✓
- [x] Namespace follows folder structure ✓
- [x] KISS: Simple in-memory implementation ✓
- [x] DRY: No code duplication ✓
- [x] YAGNI: Only required methods ✓

## Risk Assessment

### Risk 1: Event Not Firing
**Likelihood:** Low
**Impact:** High (UI won't update)
**Mitigation:** Test event invocation, use OnChanged?.Invoke() pattern

### Risk 2: Concurrent Access Issues
**Likelihood:** Low
**Impact:** Medium
**Mitigation:** Scoped services isolate per-session, no shared state

### Risk 3: Memory Leak from Event Subscriptions
**Likelihood:** Medium
**Impact:** Medium
**Mitigation:** Components must unsubscribe in Dispose (handled in Phase 03)

### Risk 4: ID Collision After Many Operations
**Likelihood:** Very Low
**Impact:** Low
**Mitigation:** int.MaxValue = 2 billion, sufficient for demo

## Security Considerations

- **Input Validation:** ArgumentNullException prevents null objects
- **Data Isolation:** Scoped services = per-session data (no cross-user access)
- **No SQL Injection:** In-memory only, no database queries
- **Event Safety:** Null-conditional operator prevents null reference exceptions
- **Future:** When adding database, add proper authorization checks

## Next Steps

1. **After Completion:** Proceed to Phase 03 (UI Component)
2. **Phase 04 will register these services:** AddScoped<T> in Program.cs
3. **Phase 03 will inject these services:** @inject directive in Transactions.razor

## Notes

- Services are stateful (in-memory storage)
- Scoped lifetime = new instance per browser session
- Events enable reactive UI (no polling needed)
- No async/await needed (in-memory operations instant)
- Future database: swap List<T> with IRepository<T>
- GetFiltered could be optimized with IQueryable for database
- CategoryTotals uses Dictionary for O(1) lookup

---

**Status:** Ready for implementation
**Next:** Create Services folder and two service files
