# AGENTS.md

ECO is a DDD/persistence library shipped as NuGet packages, with two sample apps and xUnit tests. All maintained projects target `net10.0`; CI uses the .NET 10 SDK. Do not retarget without an explicit request.

## Layout

- `src/ECO/Core/` — library packages. `ECO/` holds domain primitives and the `Data/` abstractions; provider projects implement the same persistence-unit/context/repository stack.
- `src/ECO.Tests/Core/` — three xUnit projects (core, domain events, event sourcing). There are no provider integration tests.
- `src/ECO.SampleApp/` — ASP.NET MVC sample; persistence provider is selected at compile time.
- `src/ECO.Sample.EventSourcing/` — console sample backed by Marten/PostgreSQL.
- `docs/` — user documentation. Some claims lag the code; prefer csproj and `Program.cs` as the source of truth.

## Build and test

```powershell
dotnet restore
dotnet build ECO.sln -c Release
dotnet test ECO.sln -c Release
```

Focused test run:

```powershell
dotnet test src/ECO.Tests/Core/ECO.UnitTests/ECO.UnitTests.csproj -c Release --filter "FullyQualifiedName~EntityTest"
```

- No lint, formatter, analyzer, or typecheck configuration exists. Builds already emit nullable and NuGet-audit warnings; do not clean up unrelated warnings.
- CI (`.github/workflows/eco_build.yml`) runs restore → Release build → test on `master`; CodeQL only scans `src/ECO/Core/**`.

## Important gotchas

- `ECO.sln` is incomplete. `ECO.Providers.EntityFramework.SqlServer`, `.PostgreSQL`, `.InMemory`, `ECO.Integrations.CastleWindsor`, and `ECO.Integrations.StructureMap` are not in it, so solution build/test never compiles them.
- The three excluded EF sub-provider projects currently fail to build: they reference a missing `EntityFrameworkPersistenceUnitBase`. The sample app uses the base EF provider directly. Do not add these projects to the solution unless you also fix them.
- The CastleWindsor/StructureMap projects are legacy .NET Framework 4.5.2 projects referencing a gitignored `packages/` folder.
- Package version is centralized in `src/ECO/Core/Directory.Build.props`; bump it there, not in individual csproj files.
- Package IDs differ from project/assembly names (e.g. `ECO` → `ECO.Core`, most others `ECO.Core.*`, but `ECO.Providers.MongoDB` is unprefixed). Check `<PackageId>` before referencing the package name.
- NuGet publishing happens only on GitHub Release, through NuGet Trusted Publishing (GitHub OIDC, `NuGet/login@v1`); no long-lived `NUGET_KEY` secret is used. The `deploy` job needs `id-token: write` and the repository secret `NUGET_USER` (nuget.org profile name). CI packs the projects explicitly listed in the `dotnet pack` steps; new packages must be added there to ship.

## Sample apps

- `src/ECO.SampleApp/Directory.Build.props` sets `DefineConstants` for every sample project and therefore chooses the provider: `INMEMORY`, `EFSQL`, `EFMEMORY`, `EFPOSTGRE`, `NHSQL`, `NHPOSTGRE` (current default), `MARTEN`, or `MONGODB`.
- `Program.cs` and a couple of `ECO.Sample.Application` queries use matching `#if` blocks. Switching providers requires the corresponding DAL project, a database script in `src/ECO.SampleApp/_Files/<Provider>/`, and connection strings in `appsettings.json`. Use `INMEMORY` for a no-database smoke run.
- The event-sourcing console hardcodes a Marten connection to `ECOSampleES_MT` and calls `CompletelyRemoveAllAsync()` at startup. Do not run it against a database whose data you need.

## Testing quirk

`ECO.Events.DomainEvents` is a static event bus. The domain-event test classes subscribe to the same static events and xUnit runs the classes in parallel, so a full project/solution run can intermittently fail `IDomainEventExtensionsTest.Should_raise_event_using_extension_method`. Isolate with `--filter` and re-run before assuming your change broke it.

## Architecture

Persistence flow: `IPersistenceUnitFactory` → `IDataContext` → `IPersistenceContext` → repositories. The DI entry point is `AddDataContext(options => ...)` in `ECO.Integrations.Microsoft.DependencyInjection`; providers plug in through `UseEntityFramework`, `UseNHibernate`, `UseMarten`, `UseMongoDB`, or `UseInMemory`. A new provider needs the full unit/context/manager/repository set plus a `DataContextOptions` extension; mirror an existing provider rather than inventing a new abstraction.
