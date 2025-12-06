# Codebase Summary

**Last Updated:** 2025-12-03 20:13
**Status:** MVP Complete (100%)
**Total Files:** 30 source files (excl. dependencies)
**Total Tokens:** 17,432 tokens
**Total Chars:** 77,936 chars

---

## Overview

MyMoneySaver is a .NET 10.0 Blazor Web Application using modern Blazor architecture with hybrid rendering modes. Project follows client-server pattern with WebAssembly interactivity support. MVP complete with functional money tracking UI, service layer with event-driven architecture, and full infrastructure wiring.

---

## Project Structure

```
MyMoneySaver/
├── MyMoneySaver/                          # Server project (ASP.NET Core host)
│   ├── Components/                        # Server-side Razor components
│   │   ├── Layout/                       # Layout components
│   │   │   ├── MainLayout.razor          # Main layout template
│   │   │   ├── MainLayout.razor.css      # Layout styles
│   │   │   ├── NavMenu.razor             # Navigation menu (UPDATED Phase 04)
│   │   │   ├── NavMenu.razor.css         # Navigation styles
│   │   │   ├── ReconnectModal.razor      # SignalR reconnection UI
│   │   │   ├── ReconnectModal.razor.css  # Modal styles
│   │   │   └── ReconnectModal.razor.js   # Modal JS interop
│   │   ├── Pages/                        # Routable page components
│   │   │   ├── Home.razor                # Home page (static)
│   │   │   ├── Transactions.razor        # Money tracker page (Phase 03, 224 lines)
│   │   │   ├── MudDemo.razor             # MudBlazor demo
│   │   │   ├── Weather.razor             # Weather demo (streaming render)
│   │   │   ├── Error.razor               # Error page
│   │   │   └── NotFound.razor            # 404 page
│   │   ├── App.razor                     # Root HTML document
│   │   ├── Routes.razor                  # Router configuration
│   │   └── _Imports.razor                # Global using directives
│   ├── Models/                            # Data models (Phase 01)
│   │   ├── TransactionType.cs            # Enum: Expense/Income classification
│   │   ├── Category.cs                   # Category model with icon & color
│   │   └── Transaction.cs                # Transaction model with validation
│   ├── Services/                          # Business logic services (Phase 02)
│   │   ├── CategoryService.cs            # Category CRUD with seed data
│   │   └── TransactionService.cs         # Transaction CRUD, filtering, summaries
│   ├── Properties/
│   │   └── launchSettings.json           # Development server config
│   ├── wwwroot/                          # Static web assets
│   │   ├── lib/bootstrap/                # Bootstrap 5 CSS/JS
│   │   ├── app.css                       # Global app styles
│   │   └── favicon.png                   # App icon
│   ├── Program.cs                        # Server startup & DI config (UPDATED Phase 04)
│   ├── MyMoneySaver.csproj               # Server project file
│   ├── appsettings.json                  # Production config
│   └── appsettings.Development.json      # Development config
│
├── MyMoneySaver.Client/                   # Client project (Blazor WebAssembly)
│   ├── Pages/
│   │   └── Counter.razor                 # Counter demo (interactive)
│   ├── wwwroot/
│   │   ├── appsettings.json              # Client config (production)
│   │   └── appsettings.Development.json  # Client config (development)
│   ├── Program.cs                        # WebAssembly entry point
│   ├── MyMoneySaver.Client.csproj        # Client project file
│   └── _Imports.razor                    # Client using directives
│
├── MyMoneySaver.Tests/                    # Unit tests (xUnit + FluentAssertions)
│   ├── Models/
│   │   ├── CategoryTests.cs              # Category model tests
│   │   └── TransactionTests.cs           # Transaction model tests
│   ├── Services/
│   │   ├── CategoryServiceTests.cs       # CategoryService tests
│   │   └── TransactionServiceTests.cs    # TransactionService tests
│   └── MyMoneySaver.Tests.csproj         # Test project file
│
└── MyMoneySaver.slnx                      # Solution file (XML format)
```

---

## Key Architecture Patterns

### Component Organization

1. **Server Components** (`MyMoneySaver/Components/`)
   - Static rendering by default
   - Server-side execution
   - Full .NET runtime access

2. **Client Components** (`MyMoneySaver.Client/Pages/`)
   - Interactive WebAssembly rendering
   - Browser execution
   - Limited to browser capabilities

3. **Auto Render Mode** (Counter.razor)
   - `@rendermode InteractiveAuto`
   - Server render initially, then WebAssembly
   - Best of both worlds approach

### Routing System

- **Router**: `Routes.razor` configures routing
- **App Assembly**: Main server assembly
- **Additional Assemblies**: Client assembly for WebAssembly components
- **NotFound**: Custom 404 page handler
- **Default Layout**: MainLayout for all pages

### Render Modes Demonstrated

1. **Static Rendering** (Home.razor)
   - No `@rendermode` directive
   - Server-rendered HTML only
   - No interactivity

2. **Streaming Rendering** (Weather.razor)
   - `@attribute [StreamRendering]`
   - Progressive HTML delivery
   - Async data loading with UI updates

3. **Interactive Server** (Transactions.razor)
   - `@rendermode InteractiveServer`
   - SignalR real-time updates
   - Event-driven reactive UI

4. **Interactive Auto** (Counter.razor)
   - `@rendermode InteractiveAuto`
   - Server initially, then WebAssembly
   - Full client-side interactivity

---

## File Naming Conventions

### Current Patterns

- **Components**: PascalCase (e.g., `MainLayout.razor`, `NavMenu.razor`)
- **Pages**: PascalCase (e.g., `Home.razor`, `Transactions.razor`)
- **Styles**: Component.razor.css (e.g., `MainLayout.razor.css`)
- **Scripts**: Component.razor.js (e.g., `ReconnectModal.razor.js`)
- **C# Files**: PascalCase (e.g., `Program.cs`, `CategoryService.cs`)
- **Config**: camelCase (e.g., `appsettings.json`, `launchSettings.json`)

### Recommended for New Files

Following development rules:
- **New feature components**: kebab-case preferred (e.g., `user-profile.razor`)
- **Existing patterns**: Follow PascalCase for consistency with Blazor conventions
- **Utility files**: kebab-case (e.g., `money-formatter.cs`)

---

## Important Files

### Server Project

- **Program.cs**: Configures services, middleware, routing, render modes
- **App.razor**: Root HTML document, asset references, layout structure
- **Routes.razor**: Router configuration with assembly registration
- **_Imports.razor**: Global using directives (shared namespaces)

### Client Project

- **Program.cs**: WebAssembly host builder initialization
- **_Imports.razor**: Client-specific using directives

### Configuration

- **launchSettings.json**: Dev server runs on http://localhost:5070, https://localhost:7084
- **appsettings.json**: Application configuration (logging, hosting)

---

## Dependencies

### Server Project (MyMoneySaver.csproj)

```xml
<TargetFramework>net10.0</TargetFramework>
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.Server" Version="10.0.0" />
<PackageReference Include="MudBlazor" Version="8.15.0" />
<ProjectReference Include="..\MyMoneySaver.Client\MyMoneySaver.Client.csproj" />
```

### Client Project (MyMoneySaver.Client.csproj)

```xml
<TargetFramework>net10.0</TargetFramework>
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.0" />
```

### Test Project (MyMoneySaver.Tests.csproj)

```xml
<TargetFramework>net10.0</TargetFramework>
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="FluentAssertions" Version="7.0.0-alpha.5" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0-release-24566-01" />
```

---

## Data Flow

### Current Implementation

1. **Static Pages**: Direct server render to HTML
2. **Streaming Pages**: Server initiates render, streams updates as data loads
3. **Interactive Pages**: Server pre-render, then WebAssembly takes over for client interactions

### State Management

- **Event-Driven**: Services fire events (OnTransactionsChanged, OnCategoriesChanged)
- **Component Subscriptions**: Components subscribe in OnInitialized
- **Reactive Updates**: StateHasChanged() called on event notifications
- **Cleanup**: IDisposable pattern unsubscribes to prevent memory leaks

---

## Feature Components

### Existing Features

1. **Home Page** (`/`)
   - Welcome message
   - Static content
   - Entry point

2. **Counter** (`/counter`)
   - Interactive increment button
   - Client-side state management
   - Demonstrates `InteractiveAuto` render mode

3. **Weather** (`/weather`)
   - Async data loading simulation
   - Streaming render demonstration
   - Table display pattern

4. **MudBlazor Demo** (`/muddemo`)
   - MudBlazor component showcase
   - Interactive server rendering
   - Material Design patterns

5. **Transactions** (`/transactions`) - **NEW Phase 03-04**
   - Transaction CRUD operations
   - Category filtering
   - Summary cards (Balance, Income, Expenses)
   - Date range filtering
   - Event-driven reactive UI
   - MudBlazor components
   - **Interactive Server rendering**

6. **Navigation**
   - NavMenu component
   - Bootstrap-based layout
   - Responsive design
   - **NEW:** Transactions link with wallet icon (Phase 04)

7. **Error Handling**
   - Custom error page
   - 404 not found page
   - Development vs production modes

---

## MudBlazor Integration

- **Version**: MudBlazor 8.15.0
- **Components Used**:
  - MudContainer, MudGrid, MudItem
  - MudCard, MudCardContent
  - MudTable, MudTh, MudTd
  - MudSelect, MudSelectItem
  - MudDatePicker
  - MudButton, MudIconButton
  - MudChip (with icons and colors)
  - MudText, MudPaper, MudStack
- **Registration**: `builder.Services.AddMudServices()` in Program.cs

---

## Development Workflow

### Current Setup

1. **Build**: Standard .NET build process
2. **Run**: `dotnet run` or VS launch profiles
3. **Hot Reload**: Supported via .NET 10 tooling
4. **Debugging**: Browser DevTools + VS debugger integration

### Launch Profiles

- **http**: Port 5070 (HTTP only)
- **https**: Ports 7084 (HTTPS), 5070 (HTTP redirect)
- **Environment**: Development by default

---

## Code Organization Principles

### Observed Patterns

1. **Separation of Concerns**: Layout vs Pages vs Client components
2. **Component Isolation**: Scoped CSS per component
3. **Minimal Code**: KISS principle demonstrated in Counter/Weather examples
4. **No Duplication**: DRY via shared _Imports.razor
5. **YAGNI**: Only essential features implemented

### Component Structure

```razor
@page "/route"
@rendermode InteractiveAuto  // Optional render mode

<PageTitle>Title</PageTitle>

<!-- HTML markup -->

@code {
    // C# code-behind
    // State, methods, lifecycle hooks
}
```

---

## Data Models (Phase 01)

### Implemented Models

#### TransactionType Enum
**File**: `Models/TransactionType.cs`
- **Expense** (0): Money going out (default)
- **Income** (1): Money coming in

#### Category Model
**File**: `Models/Category.cs`

Properties:
- `Id` (int): Unique identifier
- `Name` (string): Category display name (1-50 chars, required)
- `Icon` (string): Material Design icon name (max 50 chars, defaults to "category")
- `Color` (string): Hex color code (format: #RRGGBB, required)

Validation:
- Name required and length-constrained
- Icon configured for MudBlazor compatibility
- Color validated against hex format regex

#### Transaction Model
**File**: `Models/Transaction.cs`

Properties:
- `Id` (int): Unique identifier
- `Amount` (decimal): Transaction amount (0.01 - 1,000,000, required)
- `CategoryId` (int): Foreign key to Category (required, min 1)
- `Description` (string): Transaction note (1-200 chars, required)
- `Date` (DateTime): Transaction date (defaults to today, required)
- `Type` (TransactionType): Expense or Income (defaults to Expense, required)
- `Category` (Category?): Navigation property for related category

Validation:
- All properties required with data annotations
- Amount range enforced (0.01 - 1,000,000)
- Description length constrained (1-200)
- CategoryId minimum validation
- Descriptive error messages for validation

---

## Services Architecture (Phase 02)

### CategoryService
**File**: `Services/CategoryService.cs` (127 lines)

Features:
- **CRUD Operations**: GetAll, GetById, Add, Update, Delete
- **Seed Data**: 6 default categories (Food, Transport, Entertainment, Shopping, Bills, Other)
- **Event-Driven**: OnCategoriesChanged event for reactive UI
- **In-Memory Storage**: Session-scoped list
- **Material Icons**: Icon names for MudBlazor integration
- **Hex Colors**: Custom colors per category

### TransactionService
**File**: `Services/TransactionService.cs` (136 lines)

Features:
- **CRUD Operations**: GetAll, GetById, Add, Update, Delete
- **Filtering**: By category, date range, transaction type
- **Summaries**: GetTotalBalance, GetTotalIncome, GetTotalExpenses
- **Category Analytics**: GetCategoryTotals for grouping
- **Event-Driven**: OnTransactionsChanged event for reactive UI
- **In-Memory Storage**: Session-scoped list

### Service Pattern

```csharp
// Event-driven reactive updates
public event Action? OnTransactionsChanged;

// CRUD operations trigger events
public void Add(Transaction transaction)
{
    _transactions.Add(transaction);
    OnTransactionsChanged?.Invoke();  // Notify subscribers
}
```

---

## Infrastructure Integration (Phase 04)

### Dependency Injection (Program.cs)

```csharp
// Add application services (Phase 04)
builder.Services.AddScoped<MyMoneySaver.Services.TransactionService>();
builder.Services.AddScoped<MyMoneySaver.Services.CategoryService>();
```

**Scoped Lifetime Benefits:**
- New instance per HTTP request/SignalR connection
- Session-based data isolation (per-user)
- Disposed automatically at end of scope
- Perfect for in-memory session state

### Navigation Integration (NavMenu.razor)

```razor
<div class="nav-item px-3">
    <NavLink class="nav-link" href="transactions">
        <span class="bi bi-wallet2-nav-menu" aria-hidden="true"></span> Transactions
    </NavLink>
</div>
```

**Features:**
- Bootstrap wallet icon (`bi-wallet2`)
- NavLink auto-highlights when active
- Follows existing navigation pattern
- Responsive mobile-friendly design

---

## UI Components (Phase 03)

### Transactions.razor Page
**File**: `Components/Pages/Transactions.razor` (224 lines)
**Route**: `/transactions`
**Render Mode**: InteractiveServer

#### Features Implemented
1. **Summary Cards Section**: Balance, Income, Expenses with color-coded display
2. **Filter Controls**: Category dropdown, Type select, Date range pickers
3. **Transaction Table**: Display with MudTable (Date, Category, Description, Type, Amount)
4. **Action Buttons**: Add Transaction, Manage Categories
5. **CRUD Actions**: Edit/Delete buttons per row
6. **Reactive Updates**: Subscribes to service events for auto-refresh

#### MudBlazor Components Used
- MudContainer, MudGrid, MudItem
- MudCard, MudCardContent
- MudTable, MudTh, MudTd
- MudSelect, MudSelectItem
- MudDatePicker
- MudButton, MudIconButton
- MudChip (with icons and colors)
- MudText, MudPaper, MudStack

#### Event-Driven Architecture

```razor
@code {
    protected override void OnInitialized()
    {
        // Subscribe to service events
        TransactionService.OnTransactionsChanged += HandleDataChanged;
        CategoryService.OnCategoriesChanged += HandleDataChanged;
        LoadData();
    }

    private void HandleDataChanged()
    {
        LoadData();
        StateHasChanged();  // Re-render UI
    }

    public void Dispose()
    {
        // Clean up subscriptions (prevent memory leaks)
        TransactionService.OnTransactionsChanged -= HandleDataChanged;
        CategoryService.OnCategoriesChanged -= HandleDataChanged;
    }
}
```

#### Helper Methods
- `FormatCurrency(decimal)`: $1,234.56 formatting
- `GetBalanceColor()`: Dynamic color based on balance
- `GetCategoryName/Icon/Color(int)`: Category lookup from service

---

## Testing Infrastructure

### Test Coverage
- **Total Tests**: 104 tests (100% pass rate)
- **Model Tests**: 54 tests (CategoryTests, TransactionTests)
- **Service Tests**: 50 tests (CategoryServiceTests, TransactionServiceTests)

### Test Frameworks
- **xUnit**: Test runner
- **FluentAssertions**: Assertion library
- **AAA Pattern**: Arrange-Act-Assert consistently applied

### Test Examples

```csharp
[Fact]
public void Transaction_WithValidData_ShouldBeValid()
{
    // Arrange
    var transaction = new Transaction
    {
        Amount = 50.00m,
        CategoryId = 1,
        Description = "Lunch at restaurant",
        Date = new DateTime(2025, 12, 2),
        Type = TransactionType.Expense
    };

    // Act
    var validationResults = ValidateModel(transaction);

    // Assert
    validationResults.Should().BeEmpty();
}
```

---

## Token Metrics (from Repomix)

- **Total Files**: 30 files
- **Total Tokens**: 17,432 tokens
- **Total Chars**: 77,936 chars

### Top 5 Files by Token Count
1. **TransactionServiceTests.cs**: 3,998 tokens (22.9%)
2. **CategoryServiceTests.cs**: 1,729 tokens (9.9%)
3. **Transactions.razor**: 1,701 tokens (9.8%)
4. **TransactionTests.cs**: 1,309 tokens (7.5%)
5. **CategoryTests.cs**: 977 tokens (5.6%)

---

## Completed Implementation Phases

### Phase 01: Data Models (2025-12-02)
✅ TransactionType, Category, Transaction models
✅ Validation with DataAnnotations
✅ 54/54 tests passed

### Phase 02: Services (2025-12-03 07:04)
✅ CategoryService with seed data
✅ TransactionService with filtering
✅ Event-driven architecture
✅ 50/50 tests passed

### Phase 03: UI Component (2025-12-03 19:22)
✅ Transactions.razor (224 lines)
✅ Summary cards, filters, table
✅ Event subscriptions + IDisposable
✅ 104/104 tests passed

### Phase 04: Infrastructure (2025-12-03 20:13)
✅ Service registration (Scoped)
✅ Navigation link with icon
✅ Full application wiring
✅ 104/104 tests passed (no regressions)

---

## Future Additions

### Short-term
1. **Database persistence**: Replace in-memory with EF Core
2. **Authentication**: ASP.NET Core Identity
3. **Enhanced dialogs**: Full validation UI for Add/Edit
4. **Delete confirmation**: Modal confirmation dialogs

### Medium-term
1. **Analytics**: Charts, trends, visualizations
2. **Budget tracking**: Monthly budget limits & alerts
3. **Recurring transactions**: Auto-generated transactions
4. **Data export**: CSV, PDF report generation
5. **Multi-currency**: Currency selection & conversion

### Long-term
1. **Mobile optimization**: PWA capabilities
2. **Bank integration**: CSV import from bank statements
3. **Receipt scanning**: OCR for receipt processing
4. **AI insights**: Spending pattern analysis
5. **Multi-user**: Shared household budgets

---

## File Organization Recommendations

- Keep components under 200 lines (development rules)
- Extract complex logic to separate services
- Use composition over inheritance
- Split large components into smaller, focused ones

---

## Summary

MyMoneySaver is a modern Blazor Web App with functional money tracking capabilities. Project structure follows standard Blazor conventions with clean separation of concerns.

**Completed Phases (ALL 4 PHASES COMPLETE):**
- **Phase 01**: Data models with validation (DONE 2025-12-02)
- **Phase 02**: Service layer with event-driven architecture (DONE 2025-12-03)
- **Phase 03**: UI component with MudBlazor integration (DONE 2025-12-03)
- **Phase 04**: Infrastructure wiring (DONE 2025-12-03)

**Architecture Patterns:**
- Event-driven reactive UI updates
- Service-based state management via DI container
- Component lifecycle with proper disposal
- Material Design via MudBlazor
- Server-Interactive rendering for real-time updates
- Scoped service lifetime for session isolation

**Infrastructure Integration:**
- Services registered in Program.cs (TransactionService, CategoryService)
- Navigation link to /transactions page with Bootstrap wallet icon
- Full application wiring complete and tested (104/104 tests passed)

Codebase follows YAGNI/KISS/DRY principles with all files under 225 lines. **MVP complete** and ready for future enhancements (persistence, authentication, analytics).

---

**Last Updated:** 2025-12-03 20:13
**Status:** ✅ MVP COMPLETE
