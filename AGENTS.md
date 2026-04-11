# AGENTS.md

## Purpose
- This repo is a .NET 10 Clean Architecture sample with two presentation apps (Web API + MAUI) over a shared Application/Domain/Infrastructure core.
- Treat architecture tests as the source of truth for intended boundaries, but check current code for known drift before refactors.

## Repo map (start here)
- `src/CookBook.CleanArch.Domain`: entities, value objects, strongly-typed IDs, business rules (`Recipe`, `Ingredient`).
- `src/CookBook.CleanArch.Application`: MediatR commands/queries/handlers, interfaces to infra (`IRepository<,>`, `ICookBookDbContext`).
- `src/CookBook.CleanArch.Infrastructure`: EF Core DbContext, repository implementations, DI wiring, SQLite factory.
- `src/CookBook.CleanArch.Presentation.WebApi`: controllers + JSON converters for strongly typed IDs/value objects.
- `src/CookBook.CleanArch.Presentation.MauiApplication`: MVVM UI, Shell navigation, messenger-based UI refresh.

## Composition and request flow
- DI is composed in each host via extension installers: `AddUseCasesServices()` + `AddInfraServices(...)`; MAUI also calls `AddAppServices()`.
- Command/query flow is `Controller/ViewModel -> IMediator -> Handler -> Domain/Repository -> Result`.
- Commands return `Result` / `Result<T>` (no exceptions for expected validation/not-found paths).
- `UnitOfWorkBehavior` commits only for successful commands (`Application/Behaviors/UnitOfWorkBehavior.cs`).

## Domain and data patterns
- Aggregates inherit `AggregateRootBase<TId>` with strongly-typed ID records (`Domain/Shared/StronglyTypedId.cs`).
- Domain factories are static `Create(...)` methods returning `Result<T>` (example: `Domain/Ingredient/Ingredient.cs`).
- Recipe invariants are enforced in aggregate methods (max 10 ingredients, update/remove guards) (`Domain/Recipe/Recipe.cs`).
- EF model is configured by `ApplyConfigurationsFromAssembly(...)` in `CookBookDbContext`.
- Infrastructure can switch between SQLite and in-memory repositories via `DbOptions.UseInMemoryDb`.

## Project-specific conventions (important)
- Naming rules are enforced by architecture tests: `*Command`, `*Query`, `*Handler`, `*Event`, `*Controller`.
- Repositories are injected as `IRepository<TAggregate, TId>` in handlers; prefer aggregate methods over direct mutation.
- Query handlers currently use `ICookBookDbContext` + EF LINQ directly (read-side optimization pattern).
- JSON serialization requires converter factories registered in Web API (`StronglyTypedIdJsonConverterFactory`, `ValueObjectJsonConverterFactory`).

## Workflows you will use
- SDK/TFM: .NET 10 (`global.json`, `Directory.Build.props`; `LangVersion=preview`).
- Run architecture tests first when changing cross-layer code:
  - `dotnet test tests/CookBook.CleanArch.ArchitectureTests/CookBook.CleanArch.ArchitectureTests.csproj --nologo`
- Run all tests before finalizing broad changes:
  - `dotnet test CookBook.Clean.slnx --nologo`
- Run Web API locally:
  - `dotnet run --project src/CookBook.CleanArch.Presentation.WebApi/CookBook.CleanArch.Presentation.WebApi.csproj`
- API smoke scenarios live in `src/CookBook.CleanArch.Presentation.WebApi/webApi.http` and `webApiInit.http`.

## Known drift / pitfalls
- Architecture tests currently fail on two intentional/temporary mismatches:
  - Application has EF Core dependencies (`ICookBookDbContext`, query handlers).
  - Web API defines classes ending with `Factory`, conflicting with "Factories in Infrastructure" rule.
- `ArchitectureTestBase` contains old namespace constants (`CookBook.Clean.*`); assembly-based checks still run, namespace constants may mislead.
- DB path is computed from caller file path in both hosts; data file defaults to `src/CookBook.CleanArch.Infrastructure/cookbook.db`.

## Agent checklist for safe changes
- Verify which layer owns the change before editing (Domain vs Application vs Infrastructure vs Presentation).
- Preserve `Result`-based error flow and avoid introducing exception-driven control flow.
- If adding DTOs/handlers/events, follow naming conventions to avoid architecture test regressions.
- If adding new value objects or strongly typed IDs, ensure Web API JSON conversion still works.
- After edits: run targeted tests first, then full solution tests.
