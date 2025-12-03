# Codebase Summary

## Overview

MyMoneySaver is a .NET 10.0 Blazor Web Application using modern Blazor architecture with hybrid rendering modes. Project follows client-server pattern with WebAssembly interactivity support.

## Project Structure

```
MyMoneySaver/
├── MyMoneySaver/                          # Server project (ASP.NET Core host)
│   ├── Components/                        # Server-side Razor components
│   │   ├── Layout/                       # Layout components
│   │   │   ├── MainLayout.razor          # Main layout template
│   │   │   ├── MainLayout.razor.css      # Layout styles
│   │   │   ├── NavMenu.razor             # Navigation menu
│   │   │   ├── NavMenu.razor.css         # Navigation styles
│   │   │   ├── ReconnectModal.razor      # SignalR reconnection UI
│   │   │   ├── ReconnectModal.razor.css  # Modal styles
│   │   │   └── ReconnectModal.razor.js   # Modal JS interop
│   │   ├── Pages/                        # Routable page components
│   │   │   ├── Home.razor                # Home page (static)
│   │   │   ├── Transactions.razor        # Money tracker page (phase-03, 224 lines)
│   │   │   ├── Weather.razor             # Weather demo (streaming render)
│   │   │   ├── Error.razor               # Error page
│   │   │   └── NotFound.razor            # 404 page
│   │   ├── App.razor                     # Root HTML document
│   │   ├── Routes.razor                  # Router configuration
│   │   └── _Imports.razor                # Global using directives
│   ├── Models/                            # Data models (phase-01)
│   │   ├── TransactionType.cs            # Enum: Expense/Income classification
│   │   ├── Category.cs                   # Category model with icon & color
│   │   └── Transaction.cs                # Transaction model with validation
│   ├── Services/                          # Business logic services (phase-02)
│   │   ├── CategoryService.cs            # Category CRUD with seed data
│   │   └── TransactionService.cs         # Transaction CRUD, filtering, summaries
│   ├── Properties/
│   │   └── launchSettings.json           # Development server config
│   ├── wwwroot/                          # Static web assets
│   │   ├── lib/bootstrap/                # Bootstrap 5 CSS/JS
│   │   ├── app.css                       # Global app styles
│   │   └── favicon.png                   # App icon
│   ├── Program.cs                        # Server startup & configuration
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
└── MyMoneySaver.slnx                      # Solution file (XML format)
```

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

3. **Interactive Auto** (Counter.razor)
   - `@rendermode InteractiveAuto`
   - Server initially, then WebAssembly
   - Full client-side interactivity

## File Naming Conventions

### Current Patterns

- **Components**: PascalCase (e.g., `MainLayout.razor`, `NavMenu.razor`)
- **Pages**: PascalCase (e.g., `Home.razor`, `Weather.razor`, `Counter.razor`)
- **Styles**: Component.razor.css (e.g., `MainLayout.razor.css`)
- **Scripts**: Component.razor.js (e.g., `ReconnectModal.razor.js`)
- **C# Files**: PascalCase (e.g., `Program.cs`)
- **Config**: camelCase (e.g., `appsettings.json`, `launchSettings.json`)

### Recommended for New Files

Following development rules:
- **New feature components**: kebab-case preferred (e.g., `user-profile.razor`)
- **Existing patterns**: Follow PascalCase for consistency with Blazor conventions
- **Utility files**: kebab-case (e.g., `money-formatter.cs`)

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

## Dependencies

### Server Project (MyMoneySaver.csproj)

```xml
<TargetFramework>net10.0</TargetFramework>
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.Server" Version="10.0.0" />
<ProjectReference Include="..\MyMoneySaver.Client\MyMoneySaver.Client.csproj" />
```

### Client Project (MyMoneySaver.Client.csproj)

```xml
<TargetFramework>net10.0</TargetFramework>
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.0" />
```

## Data Flow

### Current Implementation

1. **Static Pages**: Direct server render to HTML
2. **Streaming Pages**: Server initiates render, streams updates as data loads
3. **Interactive Pages**: Server pre-render, then WebAssembly takes over for client interactions

### State Management

- **No centralized state**: Currently using component-local state only
- **Counter example**: Local `currentCount` variable in component `@code` block
- **Weather example**: Local `forecasts` array populated in `OnInitializedAsync`

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

4. **Navigation**
   - NavMenu component
   - Bootstrap-based layout
   - Responsive design

5. **Error Handling**
   - Custom error page
   - 404 not found page
   - Development vs production modes

## Bootstrap Integration

- **Version**: Bootstrap 5 (bundled in wwwroot/lib/bootstrap/)
- **Files**: Includes full CSS/JS, minified versions, source maps, RTL support
- **Usage**: Imported in App.razor via `@Assets` helper
- **Components**: Uses Bootstrap classes (btn, table, nav, etc.)

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

## Data Models (Phase-01)

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

## Next Steps for Expansion

### Implemented Additions

1. **Services folder**: Business logic, event-driven architecture (DONE - phase-02)
2. **Models folder**: Data models with validation (DONE - phase-01)
3. **UI Components**: Transaction management page (DONE - phase-03)

### Future Additions

1. **Shared components**: Reusable dialog components
2. **API endpoints**: Minimal APIs or controllers (if needed)
3. **Database integration**: EF Core, Dapper
4. **Authentication**: Identity, external providers
5. **State management**: Fluxor (if complex state needed)

### File Organization Recommendations

- Keep components under 200 lines (development rules)
- Extract complex logic to separate services
- Use composition over inheritance
- Split large components into smaller, focused ones

## Token Metrics (from Repomix)

- **Total Files**: 77 files
- **Total Tokens**: 3,076,188 tokens
- **Total Chars**: 8,740,329 chars
- **Largest Files**: Bootstrap CSS/JS source maps (external dependencies)
- **Core Code**: ~20 custom files (rest are libraries)

## Services Architecture (Phase-02)

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

## UI Components (Phase-03)

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

#### Current Limitations (TODOs)
- Add/Edit dialog implementation simplified (placeholders)
- Delete confirmation dialog not yet implemented
- Category management dialog not yet implemented

## Summary

MyMoneySaver is a modern Blazor Web App with functional money tracking capabilities. Project structure follows standard Blazor conventions with clean separation of concerns. Current implementation includes:

**Completed Phases:**
- **Phase 01**: Data models with validation
- **Phase 02**: Service layer with event-driven architecture
- **Phase 03**: UI component with MudBlazor integration

**Architecture Patterns:**
- Event-driven reactive UI updates
- Service-based state management
- Component lifecycle with proper disposal
- Material Design via MudBlazor
- Server-Interactive rendering for real-time updates

Codebase follows YAGNI/KISS/DRY principles with all files under 225 lines. Ready for Phase 04 (Infrastructure) and future enhancements (dialogs, persistence, authentication).
