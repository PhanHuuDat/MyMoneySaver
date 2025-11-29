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
│   │   │   ├── Weather.razor             # Weather demo (streaming render)
│   │   │   ├── Error.razor               # Error page
│   │   │   └── NotFound.razor            # 404 page
│   │   ├── App.razor                     # Root HTML document
│   │   ├── Routes.razor                  # Router configuration
│   │   └── _Imports.razor                # Global using directives
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

## Next Steps for Expansion

### Typical Additions

1. **Services folder**: Business logic, data access
2. **Models folder**: Data models, DTOs
3. **Shared components**: Reusable UI components
4. **API endpoints**: Minimal APIs or controllers
5. **Database integration**: EF Core, Dapper
6. **Authentication**: Identity, external providers
7. **State management**: Fluxor, custom services

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

## Summary

MyMoneySaver is a clean, modern Blazor Web App template with hybrid rendering capabilities. Project structure follows standard Blazor conventions with clear separation between server and client concerns. Current implementation demonstrates three key Blazor render modes (static, streaming, interactive) and provides foundation for money-saving application features.

Codebase is minimal, follows YAGNI/KISS/DRY principles, and ready for feature expansion while maintaining clean architecture.
