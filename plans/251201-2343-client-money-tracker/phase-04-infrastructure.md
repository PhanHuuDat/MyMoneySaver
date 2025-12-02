# Phase 04: Infrastructure

## Context Links

- **Parent Plan:** [plan.md](plan.md)
- **Dependencies:** Phase 02 (Services), Phase 03 (UI Component)
- **References:**
  - [Brainstorm Report](../reports/brainstorm-251201-client-side-money-tracker.md)
  - [System Architecture](../../docs/system-architecture.md)

## Overview

**Date:** 2025-12-01
**Description:** Register services in DI container and add navigation link to access Transactions page
**Priority:** P0 (Required for Functionality)
**Implementation Status:** 🔵 Not Started
**Review Status:** Pending

## Key Insights

- Services must be registered as Scoped (per-session lifetime)
- Program.cs already has MudBlazor registered
- NavMenu uses MudNavLink for navigation
- Registration happens before app.Build()
- Navigation follows existing pattern (Home, Counter, Weather)

## Requirements

### Functional
- Register TransactionService in DI container
- Register CategoryService in DI container
- Add Transactions link to navigation menu
- Link should be visible to all users
- Link should highlight when on /transactions page

### Non-Functional
- Minimal code changes (YAGNI)
- Follow existing patterns
- No breaking changes to existing functionality
- Scoped lifetime for services (session isolation)

## Architecture

### Dependency Injection Flow

```
Startup (Program.cs)
  ↓
builder.Services.AddScoped<TransactionService>()
builder.Services.AddScoped<CategoryService>()
  ↓
DI Container
  ↓
Component requests service (@inject)
  ↓
Container provides scoped instance
  ↓
Instance lives for session duration
  ↓
Session ends / Page refresh
  ↓
Instance disposed
```

### Navigation Structure

```
NavMenu.razor
├── Home (/)
├── Counter (/counter)
├── Weather (/weather)
├── MudDemo (/muddemo)
└── Transactions (/transactions) ← NEW
```

## Related Code Files

### Files to Modify
1. `MyMoneySaver/MyMoneySaver/Program.cs` (+2 lines)
2. `MyMoneySaver/MyMoneySaver/Components/Layout/NavMenu.razor` (+5 lines)

### Dependencies
- `Services/TransactionService.cs`
- `Services/CategoryService.cs`
- `Components/Pages/Transactions.razor`

## Implementation Steps

### Step 1: Register Services in Program.cs

**File:** `MyMoneySaver/MyMoneySaver/Program.cs`

**Location:** After line 19 (after `builder.Services.AddMudServices();`)

**Add:**
```csharp
// Add MudBlazor services
builder.Services.AddMudServices();

// Add application services
builder.Services.AddScoped<MyMoneySaver.Services.TransactionService>();
builder.Services.AddScoped<MyMoneySaver.Services.CategoryService>();

var app = builder.Build();
```

**Full Context:**
```csharp
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add application services
builder.Services.AddScoped<MyMoneySaver.Services.TransactionService>();
builder.Services.AddScoped<MyMoneySaver.Services.CategoryService>();

var app = builder.Build();
```

**Why Scoped:**
- Scoped = new instance per HTTP request/SignalR connection
- Session-based data isolation (per-user)
- Disposed automatically at end of scope
- Perfect for in-memory session state

**Alternative (with using):**
```csharp
using MyMoneySaver.Services;

// Then later:
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<CategoryService>();
```

### Step 2: Add Navigation Link in NavMenu.razor

**File:** `MyMoneySaver/MyMoneySaver/Components/Layout/NavMenu.razor`

**Current Structure (Reference):**
```razor
<MudNavMenu>
    <MudNavLink Href="/" Match="NavLinkMatch.All" Icon="@Icons.Material.Filled.Home">Home</MudNavLink>
    <MudNavLink Href="/counter" Match="NavLinkMatch.Prefix" Icon="@Icons.Material.Filled.Add">Counter</MudNavLink>
    <MudNavLink Href="/weather" Match="NavLinkMatch.Prefix" Icon="@Icons.Material.Filled.List">Weather</MudNavLink>
    <MudNavLink Href="/muddemo" Match="NavLinkMatch.Prefix" Icon="@Icons.Material.Filled.Dashboard">MudDemo</MudNavLink>
</MudNavMenu>
```

**Add After MudDemo Link:**
```razor
<MudNavLink Href="/transactions" Match="NavLinkMatch.Prefix" Icon="@Icons.Material.Filled.AccountBalance">
    Transactions
</MudNavLink>
```

**Final Result:**
```razor
<MudNavMenu>
    <MudNavLink Href="/" Match="NavLinkMatch.All" Icon="@Icons.Material.Filled.Home">Home</MudNavLink>
    <MudNavLink Href="/counter" Match="NavLinkMatch.Prefix" Icon="@Icons.Material.Filled.Add">Counter</MudNavLink>
    <MudNavLink Href="/weather" Match="NavLinkMatch.Prefix" Icon="@Icons.Material.Filled.List">Weather</MudNavLink>
    <MudNavLink Href="/muddemo" Match="NavLinkMatch.Prefix" Icon="@Icons.Material.Filled.Dashboard">MudDemo</MudNavLink>
    <MudNavLink Href="/transactions" Match="NavLinkMatch.Prefix" Icon="@Icons.Material.Filled.AccountBalance">
        Transactions
    </MudNavLink>
</MudNavMenu>
```

**Icon Options:**
- `Icons.Material.Filled.AccountBalance` - Money/bank icon
- `Icons.Material.Filled.AttachMoney` - Dollar sign
- `Icons.Material.Filled.Receipt` - Receipt icon
- `Icons.Material.Filled.Wallet` - Wallet icon

**Recommended:** `AccountBalance` (most appropriate for money tracking)

### Step 3: Verify Registration

**Build Project:**
```bash
dotnet build
```

**Check for:**
- No compilation errors
- Services registered successfully
- No circular dependency issues

**Run Project:**
```bash
dotnet run
```

**Test:**
1. Navigate to http://localhost:5070
2. Verify "Transactions" link appears in nav menu
3. Click link, should navigate to /transactions
4. Page should load (even if dialogs not fully implemented)

## Todo List

- [ ] Open Program.cs
- [ ] Add using statement for MyMoneySaver.Services (optional)
- [ ] Add AddScoped<TransactionService>() after MudServices
- [ ] Add AddScoped<CategoryService>()
- [ ] Open NavMenu.razor
- [ ] Add MudNavLink for /transactions with AccountBalance icon
- [ ] Save both files
- [ ] Build project (dotnet build)
- [ ] Run project (dotnet run)
- [ ] Verify nav link appears
- [ ] Click link and verify page loads
- [ ] Verify services injectable in Transactions.razor

## Success Criteria

### Compilation
- [ ] Project builds without errors
- [ ] No service registration errors
- [ ] No missing namespace errors

### Service Registration
- [ ] TransactionService registered as Scoped
- [ ] CategoryService registered as Scoped
- [ ] Services injectable via @inject directive
- [ ] Services create new instance per session
- [ ] Services disposed correctly

### Navigation
- [ ] "Transactions" link visible in nav menu
- [ ] Link icon displays correctly
- [ ] Link navigates to /transactions
- [ ] Link highlights when active
- [ ] Link works on mobile (responsive)

### Integration
- [ ] Transactions page loads successfully
- [ ] Services injected into Transactions.razor
- [ ] Summary cards display (even if zero)
- [ ] Categories list loads (seed data)
- [ ] No runtime errors in browser console

## Risk Assessment

### Risk 1: Namespace Conflicts
**Likelihood:** Low
**Impact:** Low
**Mitigation:** Use fully qualified names or add using statements

### Risk 2: Service Lifetime Issues
**Likelihood:** Low
**Impact:** Medium
**Mitigation:** Scoped is correct for session-based data

### Risk 3: Nav Link Not Rendering
**Likelihood:** Very Low
**Impact:** Low
**Mitigation:** Follow existing MudNavLink pattern exactly

### Risk 4: Services Not Found at Runtime
**Likelihood:** Low
**Impact:** High
**Mitigation:** Build first to catch registration errors

## Security Considerations

- **DI Security:** Scoped services prevent cross-user data leakage
- **Navigation:** No authorization checks (demo only, all pages public)
- **Future:** Add [Authorize] attribute when implementing auth
- **Service Isolation:** Each session gets own service instance

## Next Steps

1. **After Completion:** Full system functional
2. **Testing:** Manual test all CRUD operations
3. **Enhancement:** Implement full dialog forms
4. **Documentation:** Update README with new feature
5. **Code Review:** Verify all standards compliance

## Notes

- Service registration order doesn't matter (no dependencies between them)
- Scoped services created once per SignalR connection
- NavMenu order reflects priority (Transactions added last)
- Icon choice subjective (AccountBalance recommended)
- No breaking changes to existing functionality
- Total changes: 2 lines Program.cs + 5 lines NavMenu.razor = 7 lines

---

**Status:** Ready for implementation
**Next:** Modify Program.cs and NavMenu.razor, then test
