# EF Core Commands

Commands assume they are run from the repository root, using `Infrastructure` as the migrations project (`-p`) and `Api` as the startup project (`-s`), since `Api` is the runnable host and resolves configuration (connection strings) from its `appsettings`. Migration files are kept under `Database/Migrations` rather than the EF default location.

## Add a migration

```bash
dotnet ef migrations add <MigrationName> -p Infrastructure -s Api -o Database/Migrations
```

## Remove the last migration

Removes the most recent migration, provided it has not been applied to the database yet.

```bash
dotnet ef migrations remove -p Infrastructure -s Api
```

## Roll back the database to a previous migration

Update the database to a specific prior migration (use `0` to revert all migrations):

```bash
dotnet ef database update <PreviousMigrationName> -p Infrastructure -s Api
```
