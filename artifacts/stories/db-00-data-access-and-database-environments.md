# Story: Data Access and Database Environments

- Status: not started
- Dependency: none

## Description
Identity needs a dedicated Entity Framework Core data access project, plus a local SQL Server database for development and an Azure SQL database for a test environment, so persisted data (e.g. the client registry) has somewhere to live in every environment.

## Acceptance Criteria
- A new C# class library project exists for data access (EF Core), separate from Api/Client/App, wired into App following the `Add<Name>()` extension pattern.
- EF Core migrations can be created and applied for the project's entities.
- Connection strings are configurable per environment (not hardcoded).
- A local SQL Server database (Docker or LocalDB) is documented/scripted for developer setup, and running the app locally applies migrations against it.
- Local database connection settings live in local dev configuration (e.g. user secrets), not committed to source control.
- An Azure SQL database for the test environment is provisioned via an Azure Bicep template, deployed through a GitHub Actions workflow.
- The app can connect to and apply migrations against the Azure SQL test database using environment-specific, non-committed secrets.
- Access to the Azure SQL test database is restricted appropriately (not publicly open).

## Notes
- Local db choice (Docker vs LocalDB) to be decided during implementation planning.
- Separate migrations projects and models project.

## Open Questions
- Docker container or SQL Server LocalDB for local dev?
