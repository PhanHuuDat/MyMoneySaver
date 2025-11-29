# MyMoneySaver - AI Assistant Guide

## Project Overview

MyMoneySaver is a **Blazor Web App** built with **.NET 10.0** that uses a hybrid rendering model combining both server-side and client-side (WebAssembly) rendering capabilities. This provides the flexibility to choose the optimal rendering mode for different components.

### Tech Stack
- **Framework**: .NET 10.0
- **UI Framework**: Blazor (Hybrid Server + WebAssembly)
- **Language**: C# 12
- **CSS Framework**: Bootstrap 5
- **Nullable Reference Types**: Enabled
- **Implicit Usings**: Enabled

---

## Repository Structure

```
MyMoneySaver/
├── MyMoneySaver/                    # Main solution folder
│   ├── MyMoneySaver/                # Server project (ASP.NET Core host)
│   │   ├── Components/
│   │   │   ├── Layout/              # Layout components (MainLayout, NavMenu, etc.)
│   │   │   ├── Pages/               # Server-rendered pages (Home, Weather, Error, NotFound)
│   │   │   ├── App.razor            # Root component with HTML structure
│   │   │   ├── Routes.razor         # Router configuration
│   │   │   └── _Imports.razor       # Global using statements
│   │   ├── Properties/
│   │   ├── wwwroot/                 # Static assets
│   │   │   ├── lib/bootstrap/       # Bootstrap CSS/JS
│   │   │   ├── app.css              # Global styles
│   │   │   └── favicon.png
│   │   ├── appsettings.json
│   │   ├── Program.cs               # Server startup configuration
│   │   └── MyMoneySaver.csproj
│   │
│   └── MyMoneySaver.Client/         # Client project (Blazor WebAssembly)
│       ├── Pages/                   # Client-rendered pages (Counter)
│       ├── wwwroot/                 # Client-specific assets
│       │   ├── appsettings.json
│       │   └── appsettings.Development.json
│       ├── Program.cs               # Client startup configuration
│       ├── _Imports.razor
│       └── MyMoneySaver.Client.csproj
│
├── MyMoneySaver.slnx                # Solution file
├── .gitignore
├── .gitattributes
└── README.md
```

---

## Key Architecture Concepts

### Hybrid Rendering Model

This application supports **three rendering modes**:

1. **Static Server Rendering (SSR)**: Default for most pages (no `@rendermode`)
2. **Interactive Server**: Real-time server-side interactivity via SignalR
3. **Interactive WebAssembly**: Client-side execution in the browser
4. **InteractiveAuto**: Automatically chooses between Server and WebAssembly

**Example** (from Counter.razor:2):
```razor
@rendermode InteractiveAuto
```

### Project References

- The **server project** references the **client project** (MyMoneySaver.csproj:11)
- Server hosts and serves the WebAssembly client
- Additional assemblies are registered for routing (Program.cs:40)

---

## Development Guidelines

### File Organization

#### Where to Place New Files

- **Server-side components/pages**: `MyMoneySaver/Components/Pages/`
- **Client-side components/pages**: `MyMoneySaver.Client/Pages/`
- **Shared layout components**: `MyMoneySaver/Components/Layout/`
- **Static assets (images, CSS)**: `MyMoneySaver/wwwroot/`
- **Client-specific assets**: `MyMoneySaver.Client/wwwroot/`
- **Models/Services** (when created):
  - Shared: Create in server project, can be referenced by client
  - Client-only: Place in `MyMoneySaver.Client/`

### Naming Conventions

- **Razor components**: PascalCase with `.razor` extension (e.g., `Counter.razor`, `MainLayout.razor`)
- **CSS files**: Component-scoped CSS uses `.razor.css` (e.g., `NavMenu.razor.css`)
- **Classes/Interfaces**: PascalCase
- **Methods**: PascalCase
- **Parameters**: camelCase
- **Private fields**: camelCase (no underscore prefix)

### Component Development

#### Creating a New Page

1. Decide on the render mode (server, client, or auto)
2. Place file in appropriate location:
   - Server-only → `MyMoneySaver/Components/Pages/`
   - Client or Interactive → `MyMoneySaver.Client/Pages/`
3. Add `@page "/route"` directive
4. Set render mode if interactive: `@rendermode InteractiveAuto`
5. Update navigation in `NavMenu.razor` if needed

**Template**:
```razor
@page "/your-route"
@rendermode InteractiveAuto

<PageTitle>Your Page</PageTitle>

<h1>Your Heading</h1>

@code {
    // Component logic here
}
```

#### Component Scoped CSS

For component-specific styles, create a `.razor.css` file with the same name as your component:
- Component: `MyComponent.razor`
- Styles: `MyComponent.razor.css`

The styles are automatically scoped to that component.

### Adding Dependencies

Use the `dotnet add package` command in the appropriate project:

```bash
# Server-side package
dotnet add MyMoneySaver/MyMoneySaver/MyMoneySaver.csproj package PackageName

# Client-side package
dotnet add MyMoneySaver/MyMoneySaver.Client/MyMoneySaver.Client.csproj package PackageName
```

### Configuration

- **Server settings**: `MyMoneySaver/appsettings.json`
- **Client settings**: `MyMoneySaver.Client/wwwroot/appsettings.json`
- **Development overrides**: `appsettings.Development.json`

---

## Building & Running

### Build Commands

```bash
# Build entire solution
dotnet build MyMoneySaver.slnx

# Build specific project
dotnet build MyMoneySaver/MyMoneySaver/MyMoneySaver.csproj
```

### Run the Application

```bash
# Run from solution root
dotnet run --project MyMoneySaver/MyMoneySaver/MyMoneySaver.csproj

# Or navigate to the server project directory
cd MyMoneySaver/MyMoneySaver
dotnet run
```

The application will start on HTTPS (default: https://localhost:5001) and HTTP (default: http://localhost:5000).

### Development Mode

The app detects the environment from `ASPNETCORE_ENVIRONMENT`. In Development mode:
- WebAssembly debugging is enabled (Program.cs:22)
- Detailed error pages are shown
- No HSTS enforcement

---

## Current Features

### Existing Pages

1. **Home** (`/`) - MyMoneySaver/Components/Pages/Home.razor:1
   - Static welcome page
   - Server-rendered

2. **Counter** (`/counter`) - MyMoneySaver.Client/Pages/Counter.razor:1
   - Interactive counter demonstration
   - Uses `InteractiveAuto` render mode
   - Client-side component

3. **Weather** (`/weather`) - MyMoneySaver/Components/Pages/Weather.razor
   - Weather forecast display
   - Server-rendered

4. **Error** (`/Error`) - MyMoneySaver/Components/Pages/Error.razor
   - Error handling page

5. **Not Found** (`/not-found`) - MyMoneySaver/Components/Pages/NotFound.razor
   - 404 page (configured in Program.cs:31)

### Layout Components

- **MainLayout.razor**: Main application layout wrapper
- **NavMenu.razor**: Navigation sidebar with links to Home, Counter, and Weather
- **ReconnectModal.razor**: SignalR reconnection UI for Interactive Server mode

---

## Common Development Tasks

### Adding a New Navigation Item

Edit `MyMoneySaver/Components/Layout/NavMenu.razor`:

```razor
<div class="nav-item px-3">
    <NavLink class="nav-link" href="your-route">
        <span class="bi bi-icon-name-nav-menu" aria-hidden="true"></span> Display Name
    </NavLink>
</div>
```

### Creating a Service

1. Create a folder: `MyMoneySaver/MyMoneySaver/Services/`
2. Add your service class
3. Register in `Program.cs`:

```csharp
// Add before var app = builder.Build();
builder.Services.AddScoped<IYourService, YourService>();
```

### Working with Bootstrap

Bootstrap 5 is included via static files in `wwwroot/lib/bootstrap/`. Icons use Bootstrap Icons (bi class prefix).

---

## Important Configuration Details

### Program.cs (Server)

Key configurations in MyMoneySaver/MyMoneySaver/Program.cs:

- **Services registered** (lines 13-15):
  - Razor Components
  - Interactive Server Components
  - Interactive WebAssembly Components

- **Middleware pipeline**:
  - Status code pages with re-execute (line 31)
  - HTTPS redirection (line 32)
  - Antiforgery protection (line 34)
  - Static assets with fingerprinting (line 36)

- **Routing** (lines 37-40):
  - Maps Razor Components with all render modes
  - Includes client assembly for routing

### Project Properties

Both projects have:
- `<Nullable>enable</Nullable>` - Nullable reference types enabled
- `<ImplicitUsings>enable</ImplicitUsings>` - Common namespaces auto-imported
- `<BlazorDisableThrowNavigationException>true</BlazorDisableThrowNavigationException>` - Enhanced navigation

---

## Testing Guidelines

### When Adding Tests

1. Create test projects following .NET conventions:
   - `MyMoneySaver.Tests` for server tests
   - `MyMoneySaver.Client.Tests` for client tests

2. Use xUnit, NUnit, or MSTest
3. For Blazor component testing, consider bUnit

---

## Git Workflow

### Branches

- **Main branch**: Not explicitly configured yet
- **Feature branches**: Use the pattern `claude/descriptive-name-session-id`
- Current branch: `claude/claude-md-mijzprm6qwd3bhjq-012BA8mgkkxRcHc69uSjoFqx`

### Committing Changes

1. Review changes: `git status` and `git diff`
2. Stage relevant files: `git add <files>`
3. Commit with descriptive message:
   ```bash
   git commit -m "Add feature: description of changes"
   ```
4. Push to remote:
   ```bash
   git push -u origin <branch-name>
   ```

### Commit Message Style

Based on existing commits:
- Use present tense: "Add feature" not "Added feature"
- Be concise but descriptive
- Examples from history:
  - "Add project files."
  - "Add .gitattributes, .gitignore, and README.md."

---

## AI Assistant Best Practices

### Before Making Changes

1. **Always read files first** - Never modify code you haven't seen
2. **Understand the context** - Check related components and dependencies
3. **Verify the render mode** - Understand if working with server or client components
4. **Check existing patterns** - Follow established conventions in the codebase

### When Implementing Features

1. **Minimal changes** - Only modify what's necessary
2. **Respect the architecture** - Use the hybrid rendering model appropriately
3. **Maintain consistency** - Follow existing naming and structure patterns
4. **Avoid over-engineering** - Keep solutions simple and focused

### Code Quality

1. **Use nullable reference types** - Handle null cases properly
2. **Follow C# conventions** - Use PascalCase, proper access modifiers
3. **Component isolation** - Keep components focused and reusable
4. **CSS scoping** - Use component-scoped CSS when appropriate

### Security Considerations

1. **Validate user input** - Especially in interactive components
2. **Use Antiforgery tokens** - Already configured in Program.cs:34
3. **HTTPS enforcement** - Enabled in production
4. **Avoid hardcoded secrets** - Use configuration and secrets management

---

## Known Limitations & Future Considerations

### Current State

- This is a new project with scaffold/template code
- No custom business logic implemented yet
- No data persistence layer (database) configured
- No authentication/authorization implemented

### Typical Next Steps for Money Management App

When implementing features, consider:
1. Database setup (Entity Framework Core)
2. Authentication (ASP.NET Core Identity)
3. Models for transactions, accounts, budgets
4. Services for business logic
5. API endpoints if needed
6. State management for complex client scenarios

---

## Troubleshooting

### Common Issues

**Build Errors**:
- Clean solution: `dotnet clean`
- Restore packages: `dotnet restore`
- Rebuild: `dotnet build`

**Runtime Errors**:
- Check browser console for client-side errors
- Check server logs for server-side errors
- Verify render mode compatibility

**Routing Issues**:
- Ensure page has `@page` directive
- Verify route is unique
- Check that additional assemblies are registered (Program.cs:40)

---

## Useful Commands Reference

```bash
# Build and run
dotnet build
dotnet run --project MyMoneySaver/MyMoneySaver/MyMoneySaver.csproj

# Add packages
dotnet add package <PackageName>

# Clean build artifacts
dotnet clean

# Restore dependencies
dotnet restore

# Create new component (from project directory)
dotnet new blazorcomponent -n ComponentName

# Watch mode (auto-rebuild on file changes)
dotnet watch --project MyMoneySaver/MyMoneySaver/MyMoneySaver.csproj
```

---

## Resources

- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Bootstrap 5 Documentation](https://getbootstrap.com/docs/5.0/)
- [C# Language Reference](https://learn.microsoft.com/en-us/dotnet/csharp/)

---

**Last Updated**: 2025-11-29
**Project Version**: Initial scaffold (no custom features yet)
