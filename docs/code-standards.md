# Code Standards & Best Practices

## Core Principles

### YAGNI (You Aren't Gonna Need It)
- Implement features only when needed, not when anticipated
- Avoid speculative generalization
- Add complexity only when justified by actual requirements
- Example: Don't create abstraction layers until you have multiple implementations

### KISS (Keep It Simple, Stupid)
- Write straightforward, readable code
- Prefer clarity over cleverness
- Avoid over-engineering solutions
- Example: Use simple loops instead of complex LINQ chains when clarity suffers

### DRY (Don't Repeat Yourself)
- Extract common logic into reusable methods/components
- Use inheritance or composition to share behavior
- Centralize configuration and constants
- Example: Create shared components for repeated UI patterns

## File Naming Conventions

### Blazor Components
**Recommended**: kebab-case for new feature files (per development rules)
```
user-profile.razor
expense-list.razor
budget-summary.razor
```

**Current Pattern**: PascalCase (Blazor convention, acceptable for consistency)
```
UserProfile.razor
ExpenseList.razor
BudgetSummary.razor
```

### C# Files
**Standard**: PascalCase
```
ExpenseService.cs
BudgetCalculator.cs
MoneyFormatter.cs
```

### CSS Files
**Component Scoped**: ComponentName.razor.css
```
MainLayout.razor.css
NavMenu.razor.css
```

### JavaScript Files
**Component Scoped**: ComponentName.razor.js
```
ReconnectModal.razor.js
ChartComponent.razor.js
```

### Configuration Files
**Standard**: camelCase (JSON) or lowercase
```
appsettings.json
launchSettings.json
.gitignore
```

### General Rule
Use meaningful, descriptive names that convey purpose immediately. File names can be long if clarity demands. LLMs and developers should understand file purpose from name alone without reading content.

## File Size Management

### Size Limits
- **Maximum**: 200 lines per file (strict)
- **Target**: 100-150 lines for optimal readability
- **Minimum**: No minimum, single-purpose files encouraged

### When to Split Files

#### Components (Razor)
**Split when**:
- Component exceeds 200 lines
- Multiple responsibilities detected
- Reusable UI patterns identified

**How to split**:
```
# Before (260 lines)
ExpenseDashboard.razor

# After
ExpenseDashboard.razor          # 80 lines - layout and composition
ExpenseChart.razor              # 60 lines - chart visualization
ExpenseList.razor               # 70 lines - list display
ExpenseSummary.razor            # 50 lines - summary cards
```

#### Services (C#)
**Split when**:
- Service exceeds 200 lines
- Multiple data sources managed
- Complex business logic accumulates

**How to split**:
```
# Before (350 lines)
ExpenseService.cs

# After
ExpenseService.cs               # 100 lines - core CRUD
ExpenseCategoryService.cs       # 80 lines - category management
ExpenseCalculationService.cs    # 90 lines - calculations/aggregations
ExpenseValidationService.cs     # 80 lines - validation logic
```

#### Utility Functions
**Extract when**:
- Utility methods used across multiple files
- Mathematical calculations become complex
- Formatting logic is reusable

**Example**:
```csharp
// Extract to MoneyFormatter.cs
public static class MoneyFormatter
{
    public static string FormatCurrency(decimal amount) => $"${amount:N2}";
    public static decimal ParseCurrency(string input) => decimal.Parse(input.Replace("$", ""));
}
```

## Code Organization

### Project Structure
```
MyMoneySaver/
├── Components/
│   ├── Layout/              # Layout components only
│   ├── Pages/               # Routable pages (@page directive)
│   └── Shared/              # Reusable UI components (future)
├── Services/                # Business logic services (future)
├── Models/                  # Data models, DTOs, entities (future)
├── Data/                    # Database context, migrations (future)
├── Extensions/              # Extension methods (future)
├── Helpers/                 # Utility classes (future)
└── wwwroot/                 # Static assets

MyMoneySaver.Client/
├── Pages/                   # Interactive client pages
├── Services/                # Client-side services (future)
└── wwwroot/                 # Client-specific assets
```

### Component Structure Pattern
```razor
@page "/route"
@rendermode InteractiveAuto
@using MyMoneySaver.Services
@inject ExpenseService ExpenseService

<PageTitle>Page Title</PageTitle>

<!-- HTML Markup -->
<div class="container">
    <h1>@Title</h1>
    <!-- Component content -->
</div>

@code {
    // 1. Parameters
    [Parameter]
    public int Id { get; set; }

    // 2. Cascading Parameters
    [CascadingParameter]
    public Theme CurrentTheme { get; set; }

    // 3. Injected Services (if not using @inject)
    // [Inject] private ExpenseService ExpenseService { get; set; }

    // 4. Private Fields
    private string Title = "Expenses";
    private List<Expense> expenses = new();

    // 5. Lifecycle Methods
    protected override async Task OnInitializedAsync()
    {
        expenses = await ExpenseService.GetExpensesAsync();
    }

    // 6. Event Handlers
    private async Task HandleSubmit()
    {
        // Handle logic
    }

    // 7. Helper Methods
    private string FormatDate(DateTime date) => date.ToShortDateString();
}
```

## C# Coding Standards

### Naming Conventions

#### Classes and Interfaces
```csharp
public class ExpenseService { }          // PascalCase for classes
public interface IExpenseRepository { }  // PascalCase with 'I' prefix
public record ExpenseDto(int Id, string Name);  // PascalCase for records
```

#### Methods
```csharp
public async Task<List<Expense>> GetExpensesAsync() { }  // PascalCase, Async suffix
private void CalculateTotal() { }                        // PascalCase
```

#### Properties
```csharp
public string ExpenseName { get; set; }     // PascalCase
public int TotalAmount { get; private set; } // PascalCase
```

#### Fields
```csharp
private readonly ILogger _logger;           // camelCase with underscore prefix
private int _counter;                       // camelCase with underscore prefix
```

#### Local Variables and Parameters
```csharp
public void AddExpense(Expense expense)     // camelCase
{
    var totalAmount = 100;                  // camelCase
    int categoryId = 1;                     // camelCase
}
```

#### Constants
```csharp
private const int MaxExpenses = 1000;       // PascalCase
public const string DefaultCurrency = "USD"; // PascalCase
```

### Modern C# Features

#### Nullable Reference Types
```csharp
#nullable enable

public class Expense
{
    public string Name { get; set; } = string.Empty;  // Non-nullable, initialized
    public string? Description { get; set; }          // Nullable, explicit
}
```

#### Record Types (Data Models)
```csharp
public record ExpenseDto(int Id, string Name, decimal Amount, DateTime Date);

public record CategorySummary
{
    public string CategoryName { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
}
```

#### Pattern Matching
```csharp
public string GetExpenseStatus(Expense expense) => expense.Amount switch
{
    < 10 => "Small",
    < 100 => "Medium",
    < 1000 => "Large",
    _ => "Very Large"
};
```

#### String Interpolation
```csharp
// Preferred
var message = $"Total: {total:C}";

// Avoid
var message = "Total: " + total.ToString("C");
```

### Error Handling

#### Try-Catch Pattern
```csharp
public async Task<List<Expense>> GetExpensesAsync()
{
    try
    {
        return await _context.Expenses.ToListAsync();
    }
    catch (DbException ex)
    {
        _logger.LogError(ex, "Database error retrieving expenses");
        throw new ApplicationException("Failed to retrieve expenses", ex);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error retrieving expenses");
        throw;
    }
}
```

#### Validation Pattern
```csharp
public void AddExpense(Expense expense)
{
    ArgumentNullException.ThrowIfNull(expense);

    if (expense.Amount <= 0)
        throw new ArgumentException("Amount must be positive", nameof(expense.Amount));

    if (string.IsNullOrWhiteSpace(expense.Name))
        throw new ArgumentException("Name is required", nameof(expense.Name));

    // Process expense
}
```

### Async/Await Best Practices

#### Naming Convention
```csharp
// Correct: Suffix with Async
public async Task<List<Expense>> GetExpensesAsync() { }
public async Task SaveExpenseAsync(Expense expense) { }

// Incorrect: Missing Async suffix
public async Task<List<Expense>> GetExpenses() { }
```

#### ConfigureAwait
```csharp
// Library code: Use ConfigureAwait(false)
public async Task<Expense> GetExpenseAsync(int id)
{
    return await _repository.GetByIdAsync(id).ConfigureAwait(false);
}

// UI code (Blazor): No ConfigureAwait needed
protected override async Task OnInitializedAsync()
{
    expenses = await ExpenseService.GetExpensesAsync();
}
```

#### Avoid Async Void
```csharp
// Correct: Async Task
public async Task HandleClickAsync()
{
    await SaveDataAsync();
}

// Incorrect: Async void (only for event handlers when necessary)
public async void HandleClick()  // Avoid except for UI event handlers
{
    await SaveDataAsync();
}
```

## Blazor-Specific Standards

### Component Parameters

#### Parameter Validation
```csharp
[Parameter]
public int ExpenseId { get; set; }

[Parameter]
public EventCallback<Expense> OnExpenseUpdated { get; set; }

protected override void OnParametersSet()
{
    if (ExpenseId <= 0)
        throw new ArgumentException("ExpenseId must be positive");
}
```

#### Cascading Parameters
```csharp
[CascadingParameter]
public Task<AuthenticationState> AuthenticationStateTask { get; set; }
```

### Render Modes

#### Static Rendering (Default)
```razor
@page "/home"
<!-- No render mode = static server rendering -->
<h1>Static Page</h1>
```

#### Interactive Server
```razor
@page "/counter"
@rendermode InteractiveServer

<button @onclick="Increment">Click</button>
```

#### Interactive WebAssembly
```razor
@page "/chart"
@rendermode InteractiveWebAssembly

<AdvancedChart Data="@chartData" />
```

#### Interactive Auto (Recommended)
```razor
@page "/expenses"
@rendermode InteractiveAuto

<!-- Server-side initially, WebAssembly after download -->
<ExpenseList />
```

### Event Handling

#### Standard Pattern
```csharp
@code {
    private async Task HandleSubmit(EditContext editContext)
    {
        if (!editContext.Validate())
            return;

        await SaveExpenseAsync();
    }

    private void HandleCancel()
    {
        NavigationManager.NavigateTo("/expenses");
    }
}
```

#### EventCallback Pattern
```csharp
[Parameter]
public EventCallback<Expense> OnExpenseDeleted { get; set; }

private async Task DeleteExpense(int id)
{
    await ExpenseService.DeleteAsync(id);
    await OnExpenseDeleted.InvokeAsync(expense);
}
```

## CSS Standards

### Component Scoped CSS
```css
/* MainLayout.razor.css */
/* ::deep pseudo-selector for child elements */

.main-layout {
    display: flex;
    min-height: 100vh;
}

.main-layout .sidebar {
    width: 250px;
}

::deep .nav-item {  /* Affects child components */
    padding: 0.5rem;
}
```

### CSS Naming (BEM Convention)
```css
/* Block__Element--Modifier */
.expense-card { }
.expense-card__title { }
.expense-card__amount { }
.expense-card--highlighted { }
```

### Utility Classes (Bootstrap First)
```html
<!-- Prefer Bootstrap utilities -->
<div class="d-flex justify-content-between align-items-center">
    <span class="text-muted">Total:</span>
    <strong class="text-success">$1,234.56</strong>
</div>
```

## Documentation Standards

### XML Documentation Comments
```csharp
/// <summary>
/// Retrieves all expenses for specified date range.
/// </summary>
/// <param name="startDate">Start of date range (inclusive)</param>
/// <param name="endDate">End of date range (inclusive)</param>
/// <returns>List of expenses within date range</returns>
/// <exception cref="ArgumentException">Thrown when startDate > endDate</exception>
public async Task<List<Expense>> GetExpensesByDateRangeAsync(
    DateTime startDate,
    DateTime endDate)
{
    if (startDate > endDate)
        throw new ArgumentException("Start date must be before end date");

    return await _context.Expenses
        .Where(e => e.Date >= startDate && e.Date <= endDate)
        .ToListAsync();
}
```

### Inline Comments
```csharp
// Good: Explain WHY, not WHAT
// Using decimal for currency to avoid floating-point precision issues
private decimal CalculateTotal(List<Expense> expenses) =>
    expenses.Sum(e => e.Amount);

// Bad: Stating the obvious
// Loop through expenses and add amounts
private decimal CalculateTotal(List<Expense> expenses) =>
    expenses.Sum(e => e.Amount);
```

### Component Documentation
```razor
@*
    Expense List Component

    Displays paginated list of expenses with filtering and sorting.

    Parameters:
    - CategoryId (optional): Filter by category
    - PageSize: Items per page (default 20)

    Events:
    - OnExpenseSelected: Raised when expense clicked

    Usage:
    <ExpenseList CategoryId="5" PageSize="10" />
*@

@code {
    [Parameter]
    public int? CategoryId { get; set; }
}
```

## Testing Standards

### Unit Test Naming
```csharp
// Pattern: MethodName_Scenario_ExpectedBehavior
[Fact]
public async Task GetExpensesAsync_WithValidCategory_ReturnsFilteredExpenses()
{
    // Arrange
    var service = new ExpenseService(_mockRepository.Object);

    // Act
    var result = await service.GetExpensesAsync(categoryId: 1);

    // Assert
    Assert.NotEmpty(result);
    Assert.All(result, e => Assert.Equal(1, e.CategoryId));
}
```

### Test Organization
```csharp
public class ExpenseServiceTests
{
    private readonly Mock<IExpenseRepository> _mockRepository;
    private readonly ExpenseService _service;

    public ExpenseServiceTests()
    {
        _mockRepository = new Mock<IExpenseRepository>();
        _service = new ExpenseService(_mockRepository.Object);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public async Task GetExpenseAsync_WithValidId_ReturnsExpense(int id)
    {
        // Test implementation
    }
}
```

## Security Standards

### Input Validation
```csharp
public class CreateExpenseDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 1000000)]
    public decimal Amount { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
}
```

### SQL Injection Prevention
```csharp
// Good: Parameterized queries (EF Core does this automatically)
var expenses = await _context.Expenses
    .Where(e => e.Name.Contains(searchTerm))
    .ToListAsync();

// Bad: String concatenation
// var query = $"SELECT * FROM Expenses WHERE Name LIKE '%{searchTerm}%'";
```

### XSS Prevention
```razor
<!-- Safe: Blazor auto-encodes by default -->
<p>@expense.Description</p>

<!-- Dangerous: Only if absolutely necessary -->
<p>@((MarkupString)htmlContent)</p>
```

## Performance Standards

### Lazy Loading
```razor
@using Microsoft.AspNetCore.Components.Web.Virtualization

<Virtualize Items="@expenses" Context="expense">
    <ExpenseItem Expense="@expense" />
</Virtualize>
```

### Memo-ization for Expensive Calculations
```csharp
private decimal? _cachedTotal;
private List<Expense>? _cachedExpenses;

private decimal TotalExpenses
{
    get
    {
        if (_cachedExpenses != expenses)
        {
            _cachedTotal = expenses.Sum(e => e.Amount);
            _cachedExpenses = expenses;
        }
        return _cachedTotal.Value;
    }
}
```

### Efficient LINQ
```csharp
// Good: Single database query
var summary = await _context.Expenses
    .Where(e => e.Date >= startDate)
    .GroupBy(e => e.CategoryId)
    .Select(g => new { CategoryId = g.Key, Total = g.Sum(e => e.Amount) })
    .ToListAsync();

// Bad: Multiple queries
var expenses = await _context.Expenses.Where(e => e.Date >= startDate).ToListAsync();
var summary = expenses.GroupBy(e => e.CategoryId)
    .Select(g => new { CategoryId = g.Key, Total = g.Sum(e => e.Amount) })
    .ToList();
```

## Version Control Standards

### Commit Messages
```
feat: add expense filtering by category
fix: resolve total calculation precision issue
refactor: extract expense validation to separate service
docs: update API documentation for expense endpoints
test: add unit tests for budget calculator
chore: upgrade Bootstrap to v5.3.2
```

### Branch Naming
```
feature/expense-categories
fix/budget-calculation-bug
refactor/service-layer-cleanup
docs/api-documentation
```

### Pre-Commit Checklist
- [ ] Code compiles without errors
- [ ] No linting warnings (if linter configured)
- [ ] All tests pass
- [ ] No console.log or debug statements
- [ ] No sensitive data (API keys, passwords, connection strings)
- [ ] Code follows standards in this document

## Code Review Checklist

### Functionality
- [ ] Code implements requirements correctly
- [ ] Edge cases handled appropriately
- [ ] Error handling implemented
- [ ] Input validation present

### Code Quality
- [ ] Follows YAGNI, KISS, DRY principles
- [ ] Files under 200 lines
- [ ] Meaningful names for variables, methods, classes
- [ ] No duplicate code
- [ ] Proper use of async/await

### Testing
- [ ] Unit tests written and passing
- [ ] Test coverage adequate
- [ ] Integration tests if needed

### Security
- [ ] No SQL injection vulnerabilities
- [ ] Input properly validated and sanitized
- [ ] No hardcoded secrets
- [ ] Proper authentication/authorization

### Performance
- [ ] No N+1 query problems
- [ ] Efficient algorithms used
- [ ] Large lists use virtualization
- [ ] Unnecessary re-renders avoided

### Documentation
- [ ] Public methods have XML comments
- [ ] Complex logic explained with comments
- [ ] README updated if needed
- [ ] API documentation current

## Summary

These standards ensure maintainable, secure, performant Blazor applications. Follow YAGNI, KISS, DRY principles strictly. Keep files under 200 lines. Write clear, self-documenting code with meaningful names. Handle errors gracefully. Test thoroughly. Review code before committing.

Standards are living document - update as project evolves and new patterns emerge.
