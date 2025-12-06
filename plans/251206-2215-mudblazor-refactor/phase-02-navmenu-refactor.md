# Phase 02: MudBlazor NavMenu Refactor

**Status**: Ready for Implementation
**Created**: 2025-12-06
**Estimated Time**: 30-40 minutes
**Risk Level**: LOW
**Prerequisites**: Phase 01 (MudBlazor Cleanup) completed

---

## Objective

Replace Bootstrap-based NavMenu with Material Design navigation using MudBlazor components (MudAppBar, MudDrawer, MudNavMenu).

---

## Current State Analysis

### Existing Navigation (Bootstrap)

**File**: [NavMenu.razor](../MyMoneySaver/MyMoneySaver/Components/Layout/NavMenu.razor)

**Issues**:
1. Uses Bootstrap CSS classes (navbar, nav-item, nav-link)
2. Custom checkbox toggle for mobile menu
3. Bootstrap Icons (bi-*) instead of Material Icons
4. Non-responsive drawer behavior
5. Hardcoded styles in NavMenu.razor.css
6. Not integrated with MudBlazor layout system

**Current Routes**:
- `/` - Home
- `/counter` - Counter demo
- `/weather` - Weather demo
- `/muddemo` - MudBlazor demo
- `/transactions` - Money tracker

### MainLayout Structure

**File**: [MainLayout.razor](../MyMoneySaver/MyMoneySaver/Components/Layout/MainLayout.razor)

**Current**:
```razor
<MudThemeProvider />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />

<MudLayout>
    <MudMainContent>
        @Body
    </MudMainContent>
</MudLayout>
```

**Issue**: No MudAppBar or MudDrawer integration

---

## Solution Design

### Architecture Decision

**Pattern**: Modular Component Architecture
- **AppBar** → Separate component (`AppBar.razor`)
- **NavMenu** → Refactored component using MudDrawer + MudNavMenu
- **MainLayout** → Orchestrates AppBar + NavMenu + Content

**Rationale**:
- Follows MudBlazor best practices (Code Maze guide)
- Separation of concerns (easier to maintain)
- Reusable components
- Better testability
- Honors KISS/DRY principles

### Component Structure

```
Components/Layout/
├── MainLayout.razor          # Orchestrates AppBar + Drawer + Content
├── AppBar.razor              # Top navigation bar (NEW)
├── NavMenu.razor             # Refactored with MudDrawer + MudNavMenu
├── MainLayout.razor.css      # Layout-specific styles
└── NavMenu.razor.css         # DELETE (MudBlazor provides styling)
```

---

## Implementation Tasks

### Task 1: Create AppBar Component

**File**: `MyMoneySaver/MyMoneySaver/Components/Layout/AppBar.razor`

**Features**:
- MudAppBar with Primary color
- Hamburger menu button (toggles drawer)
- App title/logo
- Fixed to top
- Elevation 3 (subtle shadow)

**Code**:
```razor
@* Top navigation bar for MyMoneySaver *@

<MudAppBar Elevation="3" Color="Color.Primary" Fixed="true">
    <MudIconButton Icon="@Icons.Material.Filled.Menu"
                   Color="Color.Inherit"
                   Edge="Edge.Start"
                   OnClick="@ToggleDrawer" />
    <MudText Typo="Typo.h6">MyMoneySaver</MudText>
    <MudSpacer />
</MudAppBar>

@code {
    [Parameter]
    public EventCallback OnDrawerToggle { get; set; }

    private async Task ToggleDrawer()
    {
        await OnDrawerToggle.InvokeAsync();
    }
}
```

**Lines**: ~25 (within 200 line limit)

---

### Task 2: Refactor NavMenu Component

**File**: `MyMoneySaver/MyMoneySaver/Components/Layout/NavMenu.razor`

**Changes**:
- Replace Bootstrap structure with MudDrawer + MudNavMenu
- Use Material Icons instead of Bootstrap Icons
- Add proper icon mapping for routes
- Remove custom CSS dependencies
- Implement drawer open/close state

**Code**:
```razor
@* Sidebar navigation menu for MyMoneySaver *@

<MudDrawer @bind-Open="@IsOpen"
           Elevation="2"
           Variant="@DrawerVariant.Temporary"
           ClipMode="DrawerClipMode.Always">
    <MudDrawerHeader>
        <MudText Typo="Typo.h6" Color="Color.Primary">MyMoneySaver</MudText>
    </MudDrawerHeader>
    <MudNavMenu>
        <MudNavLink Href="/" Match="NavLinkMatch.All" Icon="@Icons.Material.Filled.Home">
            Home
        </MudNavLink>
        <MudNavLink Href="/counter" Icon="@Icons.Material.Filled.Add">
            Counter
        </MudNavLink>
        <MudNavLink Href="/weather" Icon="@Icons.Material.Filled.Cloud">
            Weather
        </MudNavLink>
        <MudNavLink Href="/muddemo" Icon="@Icons.Material.Filled.Palette">
            MudBlazor Demo
        </MudNavLink>
        <MudNavLink Href="/transactions" Icon="@Icons.Material.Filled.AccountBalanceWallet">
            Transactions
        </MudNavLink>
    </MudNavMenu>
</MudDrawer>

@code {
    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }
}
```

**Icon Mapping**:
| Route | Old (Bootstrap) | New (Material) |
|-------|----------------|----------------|
| Home | `bi-house-door-fill` | `Icons.Material.Filled.Home` |
| Counter | `bi-plus-square-fill` | `Icons.Material.Filled.Add` |
| Weather | `bi-list-nested` | `Icons.Material.Filled.Cloud` |
| MudDemo | `bi-palette-fill` | `Icons.Material.Filled.Palette` |
| Transactions | `bi-wallet2` | `Icons.Material.Filled.AccountBalanceWallet` |

**Lines**: ~40 (within 200 line limit)

---

### Task 3: Update MainLayout Component

**File**: `MyMoneySaver/MyMoneySaver/Components/Layout/MainLayout.razor`

**Changes**:
- Add AppBar component
- Add NavMenu with drawer state management
- Configure MudLayout with MudAppBar + MudDrawer
- Add drawer toggle callback

**Code**:
```razor
@inherits LayoutComponentBase

<MudThemeProvider />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />

<MudLayout>
    <AppBar OnDrawerToggle="@ToggleDrawer" />
    <NavMenu @bind-IsOpen="@_drawerOpen" />
    <MudMainContent>
        <div class="content">
            @Body
        </div>
    </MudMainContent>
</MudLayout>

@code {
    private bool _drawerOpen = false;

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }
}
```

**Lines**: ~30 (within 200 line limit)

---

### Task 4: Delete NavMenu.razor.css

**File**: `MyMoneySaver/MyMoneySaver/Components/Layout/NavMenu.razor.css`

**Action**: Delete entire file

**Rationale**:
- MudBlazor provides complete styling
- No custom CSS needed
- Reduces maintenance burden
- Follows YAGNI principle

---

### Task 5: Update MainLayout.razor.css (Optional)

**File**: `MyMoneySaver/MyMoneySaver/Components/Layout/MainLayout.razor.css`

**Changes** (if file exists):
- Remove any Bootstrap-dependent styles
- Add minimal content padding if needed

**Code**:
```css
.content {
    padding: 1rem;
    margin-top: var(--mud-appbar-height);
}
```

**Note**: Only create if additional styling is required

---

## Icon Reference (Material Icons)

Common icons for financial apps:

```csharp
// Navigation
Icons.Material.Filled.Home
Icons.Material.Filled.Dashboard
Icons.Material.Filled.Assessment

// Finance
Icons.Material.Filled.AccountBalanceWallet
Icons.Material.Filled.AccountBalance
Icons.Material.Filled.AttachMoney
Icons.Material.Filled.Receipt
Icons.Material.Filled.ShoppingCart

// Actions
Icons.Material.Filled.Add
Icons.Material.Filled.Edit
Icons.Material.Filled.Delete
Icons.Material.Filled.Save

// Categories
Icons.Material.Filled.Category
Icons.Material.Filled.LocalGroceryStore
Icons.Material.Filled.DirectionsCar
Icons.Material.Filled.Home (housing)
Icons.Material.Filled.Restaurant (dining)
```

---

## Verification Checklist

### Build Verification
```bash
cd MyMoneySaver/MyMoneySaver
dotnet build
```
**Expected**: 0 errors, 0 warnings

### Runtime Testing
```bash
dotnet run
```
Navigate to `https://localhost:7084/` and verify:

- [ ] AppBar displays at top with "MyMoneySaver" title
- [ ] Hamburger menu button visible
- [ ] Clicking hamburger opens/closes drawer
- [ ] Drawer displays navigation menu
- [ ] All 5 nav links present (Home, Counter, Weather, MudDemo, Transactions)
- [ ] Material icons display correctly
- [ ] Active route highlighted
- [ ] Drawer closes on route navigation (mobile behavior)
- [ ] No console errors
- [ ] No 404 errors for missing CSS

### Visual Checks
- [ ] AppBar elevation shadow visible
- [ ] Drawer slides in/out smoothly
- [ ] Icons aligned with text
- [ ] Primary color applied to AppBar
- [ ] Text legible on Primary background
- [ ] Hover states work on nav links
- [ ] Active link highlighted

### Responsive Testing
- [ ] Desktop (>1280px): Drawer overlay works
- [ ] Tablet (768-1280px): Drawer behavior correct
- [ ] Mobile (<768px): Drawer full-width overlay

---

## Rollback Plan

If issues arise:

```bash
# Restore all files
git checkout -- MyMoneySaver/MyMoneySaver/Components/Layout/

# Or restore specific files
git checkout -- MyMoneySaver/MyMoneySaver/Components/Layout/NavMenu.razor
git checkout -- MyMoneySaver/MyMoneySaver/Components/Layout/MainLayout.razor
```

---

## Implementation Order

1. Create `AppBar.razor` (new component)
2. Backup `NavMenu.razor` (optional: `git stash`)
3. Refactor `NavMenu.razor` (replace content)
4. Update `MainLayout.razor`
5. Delete `NavMenu.razor.css`
6. Build and verify compilation
7. Run application
8. Test navigation functionality
9. Verify responsive behavior

---

## Constraints & Principles

### YAGNI Applied
- No dark theme toggle (not requested)
- No user profile menu (not needed yet)
- No search functionality (future feature)
- No breadcrumbs (5 routes, not needed)

### KISS Applied
- Simple drawer toggle logic
- No complex state management
- Straightforward component hierarchy
- Minimal CSS customization

### DRY Applied
- Reusable AppBar component
- EventCallback for drawer toggle
- Consistent icon usage pattern
- Shared MudBlazor theme

---

## File Size Summary

| File | Estimated Lines | Status |
|------|----------------|--------|
| AppBar.razor | ~25 | NEW |
| NavMenu.razor | ~40 | REFACTORED |
| MainLayout.razor | ~30 | UPDATED |
| NavMenu.razor.css | - | DELETED |
| MainLayout.razor.css | ~5 | OPTIONAL |

**Total**: ~100 lines (all within 200 line limit)

---

## Dependencies

**NuGet Packages** (already installed):
- MudBlazor 8.15.0 ✅

**MudBlazor Services** (already configured):
- `AddMudServices()` in Program.cs ✅

**CSS/JS References** (already configured):
- MudBlazor.min.css ✅
- MudBlazor.min.js ✅
- Roboto font ✅

---

## Completion Criteria

- [x] AppBar.razor created
- [x] NavMenu.razor refactored with MudDrawer + MudNavMenu
- [x] MainLayout.razor updated with drawer state
- [x] NavMenu.razor.css deleted
- [x] Application builds successfully
- [ ] All routes accessible from navigation
- [ ] Drawer toggle works correctly
- [ ] Material icons display properly
- [ ] No console errors
- [ ] Responsive behavior verified

---

## References

**MudBlazor Documentation**:
- [MudAppBar](https://mudblazor.com/components/appbar)
- [MudDrawer](https://mudblazor.com/components/drawer)
- [MudNavMenu](https://mudblazor.com/components/navmenu)
- [Material Icons](https://mudblazor.com/features/icons)

**Implementation Guides**:
- [Creating Blazor Material Navigation Menu - Code Maze](https://code-maze.com/creating-blazor-material-navigation-menu/)
- [AppBar Drawer Discussion - GitHub](https://github.com/MudBlazor/MudBlazor/discussions/8261)

---

## Next Steps (Future Phases)

**Phase 03** - Dark Theme Support
- Add theme toggle button to AppBar
- Configure MudThemeProvider with custom theme
- Support user preference persistence

**Phase 04** - Category Management UI
- Add categories route to NavMenu
- Implement category CRUD operations
- Use MudDialog for forms

**Phase 05** - Dashboard Implementation
- Add dashboard route
- Create summary cards with MudCard
- Implement charts with MudCharts

---

**Plan Status**: Ready for Implementation ✅
**Blocked By**: Phase 01 completion (Console.WriteLine cleanup)
**Next Action**: Fix Phase 01 blockers, then implement Phase 02
