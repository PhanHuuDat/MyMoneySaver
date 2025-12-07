# MyMoneySaver

Personal finance management application built with .NET 10.0 Blazor Web App. Track expenses, manage budgets, and achieve savings goals with modern hybrid rendering architecture.

## Quick Start

### Prerequisites
- .NET 10.0 SDK or later
- Visual Studio 2025 / VS Code / Rider
- Modern browser (Chrome, Firefox, Safari, Edge)

### Run Locally

```bash
# Clone repository
git clone <repository-url>
cd MyMoneySaver

# Navigate to server project
cd MyMoneySaver/MyMoneySaver

# Run application
dotnet run

# Open browser
# HTTP: http://localhost:5070
# HTTPS: https://localhost:7084
```

### Development with Visual Studio
1. Open `MyMoneySaver.slnx`
2. Press F5 to run
3. Browser launches automatically

## Project Overview

**Type**: Blazor Web Application
**Framework**: .NET 10.0
**Architecture**: Hybrid (Server + WebAssembly)
**UI**: MudBlazor 8.15.0 (Material Design)
**Status**: Initial template stage

### Key Features (Planned)
- Expense tracking with categories
- Budget management and alerts
- Spending analytics and trends
- Savings goals tracking
- Multi-user support with authentication
- Responsive design (mobile-first)

## Technology Stack

### Frontend
- **Blazor Web App**: C# for client and server
- **Render Modes**: Static, Server, WebAssembly, Auto
- **UI Framework**: MudBlazor 8.15.0 (Material Design)
- **JavaScript Interop**: Minimal, as needed

### Backend
- **ASP.NET Core 10.0**: Server hosting
- **Kestrel**: High-performance web server
- **Future**: Entity Framework Core, SQL Server/PostgreSQL

### Infrastructure
- **Runtime**: .NET 10.0
- **Package Manager**: NuGet
- **Build**: MSBuild
- **Future**: Docker, Azure App Service, CI/CD

## Project Structure

```
MyMoneySaver/
├── MyMoneySaver/                      # Server project (ASP.NET Core)
│   ├── Components/
│   │   ├── Layout/                   # Layout components (MainLayout, NavMenu)
│   │   ├── Pages/                    # Routable pages (Home, Weather, Error)
│   │   ├── App.razor                 # Root HTML document
│   │   ├── Routes.razor              # Router configuration
│   │   └── _Imports.razor            # Global using directives
│   ├── Properties/
│   │   └── launchSettings.json       # Dev server config
│   ├── wwwroot/                      # Static assets (CSS, JS, images)
│   ├── Program.cs                    # Server startup
│   ├── MyMoneySaver.csproj           # Project file
│   └── appsettings.json              # Configuration
│
├── MyMoneySaver.Client/               # Client project (Blazor WebAssembly)
│   ├── Pages/
│   │   └── Counter.razor             # Interactive counter demo
│   ├── wwwroot/                      # Client-specific assets
│   ├── Program.cs                    # WebAssembly entry point
│   ├── MyMoneySaver.Client.csproj    # Client project file
│   └── _Imports.razor                # Client using directives
│
├── docs/                              # Comprehensive documentation
│   ├── project-overview-pdr.md       # Product requirements & roadmap
│   ├── codebase-summary.md           # Codebase structure guide
│   ├── code-standards.md             # Coding standards & best practices
│   └── system-architecture.md        # Architecture documentation
│
├── .claude/                           # Claude Code configuration
│   └── workflows/                    # Development workflows
│
├── MyMoneySaver.slnx                  # Solution file
├── README.md                          # This file
└── CLAUDE.md                          # AI assistant instructions
```

## Architecture Highlights

### Hybrid Rendering Strategy

**Static Rendering** (Home page)
- Server-side HTML generation
- SEO-friendly, fast first paint
- No interactivity

**Interactive Server** (Future dashboard)
- SignalR real-time connection
- Server-side state management
- Low client payload

**Interactive WebAssembly** (Future charts)
- .NET runtime in browser
- Client-side execution
- Offline capability

**Interactive Auto** (Counter demo)
- Server render first visit
- WebAssembly subsequent visits
- Best of both worlds

### Component Organization

**Server Components** (`MyMoneySaver/Components/`)
- Static and server-interactive pages
- Full .NET runtime access
- SEO-optimized content

**Client Components** (`MyMoneySaver.Client/Pages/`)
- WebAssembly interactive features
- Browser-only execution
- Rich client experiences

## Development Workflow

### Code Standards
- **Principles**: YAGNI, KISS, DRY
- **File Naming**: kebab-case preferred (PascalCase for Blazor components)
- **File Size**: Max 200 lines per file
- **Language**: C# 13 with nullable reference types
- See `docs/code-standards.md` for details

### Building
```bash
dotnet build
```

### Testing (Future)
```bash
dotnet test
```

### Publishing
```bash
dotnet publish -c Release -o ./publish
```

## Available Routes

- `/` - Home page (static)
- `/counter` - Counter demo (interactive auto)
- `/weather` - Weather demo (streaming render)
- `/muddemo` - MudBlazor components demo (interactive server)
- `/transactions` - Money tracker (interactive server) - **NEW: Full CRUD operations**

## Configuration

### Development Server
- HTTP: `http://localhost:5070`
- HTTPS: `https://localhost:7084`

### Environment Variables
Configure in `appsettings.json` or `appsettings.Development.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Security

### Current
- HTTPS enforcement (production)
- Anti-forgery token protection
- HSTS enabled (production)

### Future
- ASP.NET Core Identity authentication
- Role-based authorization
- OAuth 2.0 external providers
- JWT tokens for API access

## Performance

- **Static assets**: Fingerprinting and compression
- **Render optimization**: Multiple render modes
- **Future**: Lazy loading, virtualization, caching

## Documentation

Comprehensive docs in `docs/` directory:

- **[Project Overview & PDR](docs/project-overview-pdr.md)**: Product vision, requirements, roadmap
- **[Codebase Summary](docs/codebase-summary.md)**: Structure, conventions, key files
- **[Code Standards](docs/code-standards.md)**: Coding guidelines, patterns, best practices
- **[System Architecture](docs/system-architecture.md)**: Technical architecture, rendering, data flow
- **[MudBlazor Integration](docs/mudblazor-integration.md)**: MudBlazor setup, components, usage guide

## Roadmap

### Phase 1: Foundation (COMPLETE ✅)
- [x] Project setup
- [x] Documentation
- [x] MudBlazor integration
- [x] Data models (Phase 01)
- [x] Service layer (Phase 02)
- [x] Core CRUD operations (Phase 03)
- [x] Transaction tracking UI (Phase 03)
- [x] Infrastructure wiring (Phase 04)
- [x] **MVP COMPLETE (2025-12-03)**
- [x] **Bootstrap removal & MudBlazor cleanup (Phase 01, 2025-12-07)**

### Phase 2: Core Features
- [ ] Expense tracking
- [ ] Category management
- [ ] Budget tracking
- [ ] Dashboard with charts

### Phase 3: User Management
- [ ] Authentication
- [ ] User registration
- [ ] Profile management
- [ ] Multi-user support

### Phase 4: Analytics
- [ ] Trend analysis
- [ ] Advanced reporting
- [ ] Export functionality
- [ ] Savings goals

### Phase 5: Polish & Launch
- [ ] Mobile optimization
- [ ] Performance tuning
- [ ] Security audit
- [ ] Production deployment

See `docs/project-overview-pdr.md` for detailed roadmap.

## Contributing

### Development Rules
1. Follow YAGNI, KISS, DRY principles
2. Keep files under 200 lines
3. Write meaningful variable/method names
4. Add XML comments for public APIs
5. Handle errors with try-catch
6. Test before committing

### Commit Standards
```
feat: add expense category management
fix: resolve budget calculation precision issue
refactor: extract validation logic to service
docs: update API documentation
test: add unit tests for expense service
```

### Code Review Checklist
- [ ] Code compiles without errors
- [ ] Tests pass
- [ ] No sensitive data committed
- [ ] Follows code standards
- [ ] Documentation updated

See `docs/code-standards.md` for complete guidelines.

## License

[Specify license]

## Support

For issues or questions:
- Create GitHub issue
- Check documentation in `docs/`
- Review code standards

## Resources

### Documentation
- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [.NET 10.0 Release](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/overview)
- [MudBlazor](https://mudblazor.com/)

### Learning
- [Blazor Tutorial](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro)
- [C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

## Version

**Current**: v0.1.0 (Initial Template)
**Last Updated**: 2025-11-29

---

**Built with .NET 10.0 Blazor** | **Modern Web Development in C#**
