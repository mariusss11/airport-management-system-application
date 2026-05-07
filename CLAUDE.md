# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Airport Flight Management System — an Avalonia 11 desktop app (C# / .NET 9.0) backed by PostgreSQL. The project file is `AirportFlightManagement.csproj`; there is no `.sln`.

## Commands

```bash
dotnet restore                  # restore NuGet packages
dotnet build                    # compile (Debug)
dotnet build -c Release         # release build
dotnet run                      # launch the desktop app
```

There is no test project. Avalonia.Diagnostics is included in Debug only — press F12 in a running window to open the inspector.

### Database bootstrap

PostgreSQL must be reachable at `localhost:5432` with database `airport_flight_management`, user/password `postgres`/`postgres` (hardcoded in `Services/DatabaseService.cs:15`). Apply schema in this order:

```bash
psql -U postgres -d airport_flight_management -f Database/migration_001_add_airports.sql
psql -U postgres -d airport_flight_management -f Database/insert.sql   # sample seed data
```

The base tables (`flights`, `archived_flights`, `planes`, `users`) are not in a versioned schema file — they must already exist when the migration runs. The schema is documented in `BUILD_STATUS.md` lines 138–182.

## Architecture

### MVVM with convention-based view resolution

`ViewLocator` (`ViewLocator.cs`) maps any `ViewModelBase` instance to its View by string substitution: `FooViewModel` → `FooView`. Bindings hit it through `Application.DataTemplates` in `App.axaml`. **Renaming a ViewModel without renaming its View will silently render `Not Found: ...`** — keep the pair in sync.

All ViewModels derive from `ViewModelBase` (a thin `ObservableObject` subclass) and use `CommunityToolkit.Mvvm` source generators (`[ObservableProperty]`, `[RelayCommand]`).

### Navigation

`MainWindowViewModel.CurrentPage` holds the active ViewModel; the sidebar in `Views/MainWindow.axaml` is wired through code-behind (`MainWindow.axaml.cs:OnNavClick`), which `switch`es on `Button.Tag` strings to instantiate the right ViewModel. **New screens require updating both the XAML sidebar and the `OnNavClick` switch** — there is no DI container.

### Data layer

`DatabaseService` is a manual singleton (`DatabaseService.Instance`) that:
- Opens a fresh `NpgsqlConnection` per query (no pooling logic of its own — relies on Npgsql defaults).
- Hydrates `ObservableCollection<T>` properties (`Flights`, `Planes`, `Airports`, `Users`, `ArchivedFlights`) once at construction via `LoadDataFromDatabase()`. Each loader is wrapped in `TryLoad` so a missing table for one entity doesn't break the others; the last error lands in `LastError`.
- Mutating methods (e.g. `AddFlight`, `UpdateFlight`, `DeleteFlight`) write to PostgreSQL **and** update the in-memory collection. ViewModels that snapshot these into their own `ObservableCollection` (e.g. `FlightsViewModel.Flights`) must reassign after a mutation — see `FlightsViewModel.SaveFlight` for the pattern.
- `DeleteFlight` is soft-delete: it inserts into `archived_flights` and removes from `flights` in the same connection.

### Authentication

`AuthService.Login` verifies BCrypt hashes against `DatabaseService.Instance.Users`. Demo creds: `admin` / `password`. In **Debug builds**, `MainWindowViewModel` auto-logs in as admin and lands on the dashboard (`MainWindowViewModel.cs:22-35`) — the login screen only appears in Release.

### Modal dialog pattern

`Views/EditFlightModal` is a `Window` opened from `FlightsView` code-behind via `ShowDialog`. It shares the parent `FlightsViewModel` as its `DataContext` and listens to `IsModalOpen` (`PropertyChanged`) to self-close. When adding similar modals, follow this shared-VM convention rather than constructing a new VM — the dialog reuses the parent's command bindings and field state.

### Styling

`App.axaml` forces `RequestedThemeVariant="Dark"` and pulls `Styles/DarkTheme.axaml`, which defines the BMW-M-inspired palette (deep navy `#0A0E27`, M-Sport red `#DC143C`, tech blue `#00B4D8`). Common button classes: `primary`, `secondary`, `danger`, `outline`. Card variants: `Border.card`, `Border.stat-card`. See `DESIGN_SPEC.md` for the full design system.

### Compiled bindings are OFF

`AvaloniaUseCompiledBindingsByDefault=false` (`AirportFlightManagement.csproj:8`). Bindings resolve at runtime, so binding errors won't surface at build time — watch the debug console for `BindingError` messages. Do not flip this flag without auditing every `.axaml`; many bindings rely on duck-typed property lookup that compiled bindings would reject.

## Conventions worth knowing

- `Models/Flight` exposes a derived `Duration` (minutes) and `DayName` — don't store these in the DB.
- All flight-related times are `TimeOnly`; dates are `DateOnly`. Npgsql 10 maps these natively via `GetFieldValue<TimeOnly>` / `GetFieldValue<DateOnly>`.
- Form validation lives inline in ViewModels (e.g. `FlightsViewModel.SaveFlight` lines 173–325). There is no validation framework — extend the existing pattern rather than introducing FluentValidation/DataAnnotations.
- `bmw-m/` and `Folder.DotSettings.user` are local design/IDE artifacts; don't reference them from code.
