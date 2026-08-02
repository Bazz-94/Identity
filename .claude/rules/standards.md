# Standards

## Project Standards
- The project adheres to CLEAN architecture principles, with a clear separation of concerns between the Domain, Application, Infrastructure, and API layers.
- Client (UI) pages never call App-layer services directly. UI talks to Api controllers over HTTP; controllers call App-layer services. Keeps frontend/backend separation of concerns explicit.

## C# Standards

- No `var` — always explicit types.
- Use `this.` for instance members; always use block bodies for methods.
- Stateful types expose private setters plus explicit state-transition methods (e.g. a `Player` with a private-set `Health` mutated only via `TakeDamage`/`Heal`, never by setting the field directly).
- Avoid single-use local variables — inline the expression instead.
- Define constants or Enums for meaningful values instead of hardcoding them; never hardcode string values. Exception: seed data values don't need constants unless the same value is duplicated elsewhere.
- Avoid redundant words in names (`GameLoop`, not `RiverRunGameLoop`, when context is already clear).
- Prefer `foreach` over `for`.
- Provide concise XML doc comments on all classes, methods, and properties.
- Unit test all Domain logic; Application and Infrastructure code may be untested or covered by integration tests only.