# System Architecture

## Architecture Overview

MyMoneySaver uses **Blazor Hybrid Architecture** combining server-side rendering with client-side WebAssembly interactivity. This approach balances performance, SEO, and rich user experiences.

**UI Framework**: MudBlazor 8.15.0 - Material Design components for Blazor

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                         Client Browser                       │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Static HTML (SSR)                                      │ │
│  │  - Initial page load                                    │ │
│  │  - SEO-friendly content                                 │ │
│  │  - Fast first paint                                     │ │
│  └────────────────────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Blazor WebAssembly                                     │ │
│  │  - .NET runtime in browser                              │ │
│  │  - Client-side interactivity                            │ │
│  │  - Offline capability (future)                          │ │
│  └────────────────────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  SignalR Connection (Server Mode)                       │ │
│  │  - Real-time updates                                    │ │
│  │  - Server-side state management                         │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ HTTPS
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    ASP.NET Core Server                       │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Kestrel Web Server                                     │ │
│  │  - HTTP/HTTPS endpoints                                 │ │
│  │  - Static file serving                                  │ │
│  │  - WebSocket support                                    │ │
│  └────────────────────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Blazor Server Runtime                                  │ │
│  │  - Component rendering                                  │ │
│  │  - SignalR hub                                          │ │
│  │  - Server-side interactivity                            │ │
│  └────────────────────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Business Logic Layer                                   │ │
│  │  - Services (future)                                    │ │
│  │  - Validation                                           │ │
│  │  - Business rules                                       │ │
│  └────────────────────────────────────────────────────────┘ │
│  ┌────────────────────────────────────────────────────────┐ │
│  │  Data Access Layer (future)                             │ │
│  │  - Entity Framework Core                                │ │
│  │  - Repository pattern                                   │ │
│  │  - LINQ queries                                         │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ Connection String
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                     Database (Future)                        │
│  - SQL Server / PostgreSQL / SQLite                          │
│  - Tables: Expenses, Categories, Users, Budgets              │
│  - Indexes, constraints, stored procedures                   │
└─────────────────────────────────────────────────────────────┘
```

## Rendering Architecture

### Blazor Render Modes

MyMoneySaver utilizes three distinct rendering strategies:

#### 1. Static Server Rendering (SSR)
**Components**: Home.razor, Error.razor, NotFound.razor

```
Browser Request → Server generates HTML → Response sent → Page displayed
                                                         (No interactivity)
```

**Characteristics**:
- Fastest initial load
- SEO-friendly (crawlable HTML)
- No client-side state
- Server-rendered on every request
- Best for content pages

**Implementation**:
```razor
@page "/"
<!-- No @rendermode directive = static -->
<h1>Welcome to MyMoneySaver</h1>
```

#### 2. Interactive Server Rendering
**Use Case**: Real-time updates, server-side state

```
Browser Request → Initial HTML → SignalR WebSocket → DOM updates via SignalR
                                       ↑
                                  Server executes
                                  C# event handlers
```

**Characteristics**:
- Immediate interactivity after initial load
- Server maintains component state
- Requires persistent connection
- Low client-side payload
- Higher server resource usage

**Implementation**:
```razor
@page "/dashboard"
@rendermode InteractiveServer

<button @onclick="RefreshData">Refresh</button>
```

#### 3. Interactive WebAssembly Rendering
**Components**: Counter.razor (via InteractiveAuto)

```
Browser Request → Initial HTML + WASM download → .NET runtime loads
                                                → Client executes C#
```

**Characteristics**:
- Full client-side execution
- Offline capability (once cached)
- No server round-trips for interactions
- Larger initial download (~2MB)
- Best for rich, interactive features

**Implementation**:
```razor
@page "/counter"
@rendermode InteractiveWebAssembly

<button @onclick="IncrementCount">Click me</button>
```

#### 4. Interactive Auto (Recommended Default)
**Current**: Counter.razor
**Strategy**: Best of both worlds

```
First Visit:
Browser → Initial HTML → SignalR connection → Server-side interactivity
                                             → WASM downloads in background

Subsequent Visits:
Browser → Initial HTML → WASM runtime executes immediately (cached)
```

**Characteristics**:
- Server render on first visit (fast)
- WebAssembly on subsequent visits (rich)
- Automatic mode selection
- Progressive enhancement
- Optimal user experience

**Implementation**:
```razor
@page "/expenses"
@rendermode InteractiveAuto

<!-- Server-side first time, WebAssembly after -->
<ExpenseList />
```

### Render Mode Decision Matrix

| Feature Type | Recommended Mode | Reason |
|-------------|------------------|---------|
| Landing pages | Static | SEO, performance |
| Dashboard | InteractiveAuto | Balance of speed and interactivity |
| Data entry forms | InteractiveAuto | Validation, state management |
| Charts/graphs | InteractiveWebAssembly | Heavy client-side processing |
| Reports | Static or Server | Server-side data aggregation |
| Real-time updates | InteractiveServer | SignalR built-in |
| Offline features | InteractiveWebAssembly | Client-side execution |

## Project Structure & Dependencies

### Dual-Project Architecture

```
MyMoneySaver.sln
├── MyMoneySaver              (Server Project)
│   ├── Target: net10.0
│   ├── SDK: Microsoft.NET.Sdk.Web
│   └── Dependencies:
│       ├── Microsoft.AspNetCore.Components.WebAssembly.Server@10.0.0
│       └── Project Reference: MyMoneySaver.Client
│
└── MyMoneySaver.Client       (Client Project)
    ├── Target: net10.0
    ├── SDK: Microsoft.NET.Sdk.BlazorWebAssembly
    └── Dependencies:
        └── Microsoft.AspNetCore.Components.WebAssembly@10.0.0
```

### Assembly Registration

**Program.cs (Server)**:
```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()       // Enable server rendering
    .AddInteractiveWebAssemblyComponents(); // Enable WASM rendering

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);  // Register client assembly
```

This configuration allows:
- Server to render components from both projects
- Router to discover routes in client assembly
- Seamless transition between render modes

## Routing System

### Router Configuration

**Routes.razor**:
```razor
<Router AppAssembly="typeof(Program).Assembly"
        AdditionalAssemblies="new[] { typeof(Client._Imports).Assembly }"
        NotFoundPage="typeof(Pages.NotFound)">
    <Found Context="routeData">
        <RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)" />
        <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
</Router>
```

### Route Resolution Flow

```
1. Browser navigates to /counter
   ↓
2. Router scans both assemblies for @page "/counter"
   ↓
3. Found in MyMoneySaver.Client/Pages/Counter.razor
   ↓
4. Check render mode (@rendermode InteractiveAuto)
   ↓
5. First visit: Render on server via SignalR
   Subsequent: Render via WebAssembly
   ↓
6. Apply MainLayout (wrapper)
   ↓
7. Display component
   ↓
8. Focus on <h1> for accessibility
```

### Route Registration

Routes auto-discovered via `@page` directive:

```razor
@page "/expenses"              → /expenses
@page "/expenses/{id:int}"     → /expenses/123
@page "/categories/{name}"     → /categories/food
```

## Data Flow Patterns

### Current: Component-Local State

```
Component
  ↓
  State stored in @code block (private fields)
  ↓
  Lifecycle methods modify state
  ↓
  UI updates automatically (re-render)
```

**Example** (Counter.razor):
```csharp
@code {
    private int currentCount = 0;  // Component-local state

    private void IncrementCount()
    {
        currentCount++;            // Modify state
    }                              // UI re-renders automatically
}
```

### Future: Service-Based State

```
Component
  ↓
  Inject Service
  ↓
  Service manages state
  ↓
  Component subscribes to changes
  ↓
  UI updates on state change notification
```

**Planned Pattern**:
```csharp
@inject ExpenseService ExpenseService

@code {
    private List<Expense> expenses = new();

    protected override async Task OnInitializedAsync()
    {
        expenses = await ExpenseService.GetExpensesAsync();
        ExpenseService.OnExpensesChanged += HandleExpensesChanged;
    }

    private void HandleExpensesChanged()
    {
        StateHasChanged();  // Trigger UI re-render
    }
}
```

### Future: Centralized State Management (Fluxor)

```
Component → Dispatch Action
              ↓
         Fluxor Store
              ↓
         Reducer modifies state
              ↓
         Component subscribes to state
              ↓
         UI updates automatically
```

## Middleware Pipeline

### Current Pipeline (Program.cs)

```csharp
// 1. Development: Enable WebAssembly debugging
if (app.Environment.IsDevelopment())
    app.UseWebAssemblyDebugging();

// 2. Production: Exception handling
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// 3. Status code pages (404, 500, etc.)
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// 4. HTTPS redirection
app.UseHttpsRedirection();

// 5. Anti-forgery token validation
app.UseAntiforgery();

// 6. Static files (wwwroot)
app.MapStaticAssets();

// 7. Razor components routing
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
```

### Request Processing Flow

```
Client Request
  ↓
HTTPS Redirection (if HTTP)
  ↓
Static File Check (wwwroot)
  ├─ Found → Serve file → End
  └─ Not Found
      ↓
  Antiforgery Validation (for forms)
      ↓
  Router Match
  ├─ Match Found → Render Component
  └─ No Match → Status Code Page (404)
      ↓
  Return Response
```

## Security Architecture

### Current Implementation

#### HTTPS Enforcement
- **Development**: HTTP (5070) and HTTPS (7084)
- **Production**: HTTPS only via `UseHttpsRedirection()`

#### Anti-Forgery Protection
```csharp
app.UseAntiforgery();  // Protects forms from CSRF attacks
```

**Usage in Forms**:
```razor
<EditForm Model="expense" OnValidSubmit="HandleSubmit">
    <!-- Antiforgery token added automatically -->
</EditForm>
```

#### HSTS (HTTP Strict Transport Security)
```csharp
app.UseHsts();  // Enforces HTTPS in browsers (production only)
```

### Future Security Layers

#### Authentication (Planned)
```
User → Login Form → ASP.NET Core Identity
                    ↓
               Validate credentials
                    ↓
               Issue authentication cookie/JWT
                    ↓
               Authorize access to protected pages
```

#### Authorization (Planned)
```csharp
@attribute [Authorize]  // Require authentication
@attribute [Authorize(Roles = "Admin")]  // Role-based

// In service
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("CanEditExpenses", policy =>
        policy.RequireClaim("Permission", "EditExpenses"));
});
```

#### Data Protection
- **At Rest**: Database encryption (TDE, field-level)
- **In Transit**: TLS 1.2+ (HTTPS)
- **Secrets**: Azure Key Vault / Environment variables

## Configuration Management

### Configuration Hierarchy

```
1. appsettings.json                    (Base configuration)
2. appsettings.{Environment}.json      (Environment overrides)
3. User Secrets (Development)          (Local developer secrets)
4. Environment Variables               (Deployment configuration)
5. Azure Key Vault (Production)        (Sensitive production secrets)
```

### Current Configuration Files

**appsettings.json** (Server):
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**appsettings.Development.json**:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Client Configuration** (wwwroot/appsettings.json):
```json
{
  // Client-specific settings (API endpoints, feature flags)
}
```

### Accessing Configuration

```csharp
// Server-side
public class ExpenseService
{
    private readonly IConfiguration _configuration;

    public ExpenseService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void UseConfig()
    {
        var connString = _configuration["ConnectionStrings:DefaultConnection"];
        var apiKey = _configuration["ExternalApi:ApiKey"];
    }
}

// Client-side (WebAssembly)
@inject IConfiguration Configuration

@code {
    protected override void OnInitialized()
    {
        var apiUrl = Configuration["ApiBaseUrl"];
    }
}
```

## Error Handling Architecture

### Error Boundaries

```
Component
  ↓
Exception Thrown
  ↓
ErrorBoundary Catches
  ↓
Display Fallback UI
  ↓
Log Error
```

**Usage**:
```razor
<ErrorBoundary>
    <ChildContent>
        <ExpenseList />
    </ChildContent>
    <ErrorContent Context="error">
        <p>Something went wrong: @error.Message</p>
    </ErrorContent>
</ErrorBoundary>
```

### Global Error Handling

**Production**:
```csharp
app.UseExceptionHandler("/Error");  // Redirect to Error.razor
```

**Error.razor**:
```razor
@page "/error"
@attribute [IgnoreAntiforgeryToken]

<h1>Error</h1>
<p>An error occurred while processing your request.</p>
```

### Logging Architecture

```
Application → ILogger<T>
                ↓
          Logging Provider
                ↓
          ┌─────┴─────┐
          │           │
      Console      File
          │           │
          └─────┬─────┘
                ↓
         Azure App Insights (Future)
```

## Performance Optimization Patterns

### Static Asset Optimization

**MapStaticAssets()**:
- Fingerprinting (cache busting)
- Compression (gzip, brotli)
- CDN integration ready
- Browser caching optimization

**Usage in App.razor**:
```razor
<link rel="stylesheet" href="@Assets["app.css"]" />
<!-- Generates: app.css?v=<hash> for cache busting -->
```

### Component Lifecycle Optimization

```
OnInitialized/OnInitializedAsync    → One-time initialization
  ↓
OnParametersSet/OnParametersSetAsync → Parameter change handling
  ↓
ShouldRender()                       → Render optimization
  ↓
OnAfterRender/OnAfterRenderAsync    → DOM manipulation
```

**Optimization Example**:
```csharp
protected override bool ShouldRender()
{
    // Only re-render if data actually changed
    return _hasDataChanged;
}
```

### Lazy Loading (Future)

**Route-based**:
```razor
<Router>
    <Found>
        @if (routeData.PageType == typeof(HeavyComponent))
        {
            <Suspense Fallback="<p>Loading...</p>">
                <DynamicComponent Type="routeData.PageType" />
            </Suspense>
        }
    </Found>
</Router>
```

## Deployment Architecture

### Current Setup (Development)

```
Developer Machine
  ↓
dotnet run / Visual Studio
  ↓
Kestrel Server (localhost:5070, localhost:7084)
  ↓
Browser
```

### Future Production Deployment

```
Code Repository (GitHub)
  ↓
CI/CD Pipeline (GitHub Actions)
  ↓
dotnet publish -c Release
  ↓
Docker Container / Azure App Service
  ↓
Load Balancer
  ↓
Multiple App Instances (Scaling)
  ↓
CDN (Static Assets)
  ↓
End Users
```

**Hosting Options**:
1. **Azure App Service**: PaaS, easy deployment
2. **Docker + Kubernetes**: Container orchestration
3. **IIS**: Traditional Windows hosting
4. **Linux + Nginx**: Reverse proxy setup

## Data Model Architecture (Phase-01)

### Core Models Implemented

```csharp
// TransactionType Enum
public enum TransactionType
{
    Expense = 0,      // Money going out
    Income = 1        // Money coming in
}

// Category Model
public class Category
{
    public int Id { get; set; }                    // Primary Key
    public string Name { get; set; }               // 1-50 chars (required)
    public string Icon { get; set; }               // Material Design icon name
    public string Color { get; set; }              // Hex format (#RRGGBB)
}

// Transaction Model
public class Transaction
{
    public int Id { get; set; }                    // Primary Key
    public decimal Amount { get; set; }            // 0.01 - 1,000,000
    public int CategoryId { get; set; }            // Foreign Key
    public string Description { get; set; }        // 1-200 chars
    public DateTime Date { get; set; }             // Defaults to today
    public TransactionType Type { get; set; }      // Expense or Income
    public Category? Category { get; set; }        // Navigation property
}
```

### Model Validation

All models use `System.ComponentModel.DataAnnotations`:

**Category Validation**:
- `Name`: Required, length 1-50
- `Icon`: Required, max length 50
- `Color`: Required, regex validation for hex format (#RRGGBB)

**Transaction Validation**:
- `Amount`: Required, range 0.01 - 1,000,000
- `CategoryId`: Required, minimum value 1
- `Description`: Required, length 1-200
- `Date`: Required, defaults to `DateTime.Today`
- `Type`: Required, defaults to `TransactionType.Expense`

### Model Organization

Location: `MyMoneySaver/Models/`

Files:
1. `TransactionType.cs`: Enum for type classification
2. `Category.cs`: Category entity with Material Design support
3. `Transaction.cs`: Transaction entity with full validation

### Data Flow Architecture

```
Blazor Component
  ↓
Form/Edit Component (with EditForm)
  ↓
Data Validation (Annotations on Model)
  ├─ Client-side validation (EditForm in interactive modes)
  └─ Server-side validation (required for security)
  ↓
Service Layer (Future)
  ↓
Repository Pattern (Future - EF Core)
  ↓
Database Context
  ↓
Database
```

### Entity Relationship (Phase-01)

```
Categories (1 → Many) Transactions
  ├─ One Category has many Transactions
  └─ Transaction.CategoryId references Category.Id
```

Current implementation is model-only (no database yet). Models are ready for:
- Entity Framework Core DbContext setup
- Database migrations
- CRUD operations via repositories

## Database Architecture (Future)

### Planned Architecture

```
Application Layer (Blazor Server)
  ↓
Service Layer
  ↓
Repository Pattern (IExpenseRepository)
  ↓
Entity Framework Core (DbContext)
  ↓
Database Provider (SQL Server / PostgreSQL)
  ↓
Database
```

### Extended Entity Relationship (Planned)

```
Users
  ├─ Id (PK)
  ├─ Email
  ├─ PasswordHash
  └─ CreatedDate

Categories
  ├─ Id (PK)
  ├─ Name
  ├─ Icon
  └─ Color

Transactions (Currently "Expenses")
  ├─ Id (PK)
  ├─ UserId (FK → Users)
  ├─ CategoryId (FK → Categories)
  ├─ Amount
  ├─ Description
  ├─ Date
  ├─ Type (TransactionType)
  └─ CreatedDate

Budgets
  ├─ Id (PK)
  ├─ UserId (FK → Users)
  ├─ CategoryId (FK → Categories)
  ├─ Amount
  ├─ StartDate
  └─ EndDate
```

### Data Access Pattern (Planned)

```csharp
// Repository
public interface IExpenseRepository
{
    Task<List<Expense>> GetExpensesAsync(int userId);
    Task<Expense> GetByIdAsync(int id);
    Task AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(int id);
}

// Service
public class ExpenseService
{
    private readonly IExpenseRepository _repository;

    public async Task<List<Expense>> GetUserExpensesAsync(int userId)
    {
        return await _repository.GetExpensesAsync(userId);
    }
}

// Component
@inject ExpenseService ExpenseService

@code {
    private List<Expense> expenses = new();

    protected override async Task OnInitializedAsync()
    {
        expenses = await ExpenseService.GetUserExpensesAsync(currentUserId);
    }
}
```

## Scalability Considerations

### Horizontal Scaling

**Server Components**:
- Stateless server rendering scales easily
- Multiple instances behind load balancer
- Session affinity for SignalR (sticky sessions)

**WebAssembly Components**:
- Client-side execution (no server load)
- Scales infinitely (runs on user's device)

### Caching Strategy (Future)

```
Component Request
  ↓
Check Memory Cache
  ├─ Hit → Return data
  └─ Miss
      ↓
  Check Distributed Cache (Redis)
  ├─ Hit → Return data + Update memory cache
  └─ Miss
      ↓
  Query Database
      ↓
  Update caches
      ↓
  Return data
```

### SignalR Scaling (Future)

For multiple server instances:
- Azure SignalR Service (managed scaling)
- Redis backplane (self-hosted)

## Summary

MyMoneySaver architecture leverages modern Blazor capabilities:
- **Hybrid rendering** for optimal performance and UX
- **Dual-project structure** for clear separation of concerns
- **Flexible render modes** tailored to each feature's needs
- **Middleware pipeline** providing security and error handling
- **Scalable design** ready for growth

Current implementation is minimal template. Future additions (authentication, database, state management) will integrate cleanly into established architecture without major refactoring.

Architecture prioritizes:
- Developer productivity (C# everywhere)
- Performance (multiple render strategies)
- Maintainability (clean separation, YAGNI/KISS/DRY)
- Security (HTTPS, antiforgery, future auth)
- Scalability (stateless design, horizontal scaling ready)
