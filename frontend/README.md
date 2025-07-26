# JobSage.BlazorApp

This is a .NET 9.0 Blazor WebAssembly front-end application scaffolded with Clean Code and Domain-Driven Design (DDD) principles.

## Key Features
- Project names and namespaces start with JobSage.*
- PostgreSQL is used for data storage and access
- Structure and practices are inspired by [JobSageSolution](https://github.com/shishir28/JobSageSolution), adapted for JobSage
- Organized into Domain, Application, Infrastructure, and Presentation layers
- Clean Code, separation of concerns, and testability

## Getting Started
1. Ensure you have .NET 9.0 SDK installed
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Build the project:
   ```sh
   dotnet build
   ```
4. Run the app:
   ```sh
   dotnet run --project JobSage.BlazorApp.csproj
   ```

## Configuration
- Use environment variables and configuration files for secrets and connection strings
- Update PostgreSQL connection settings in `appsettings.json`

## Next Steps
- Implement DDD layers and PostgreSQL integration
- Add domain models, repositories, and services
- Build UI components and pages

---
For workspace-specific Copilot instructions, see `.github/copilot-instructions.md`.
