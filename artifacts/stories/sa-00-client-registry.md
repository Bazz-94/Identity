# Story: Client App Registry

- Status: implemented
- Dependency: db-00-data-access-and-database-environments

## Description
Identity needs to know which client apps are allowed to use it for SSO. Each client app is registered with a client id, client secret, and an allowed redirect domain. This registry is the foundation the login and token flows check against.

## Acceptance Criteria
- A client app can be registered with: client id, client secret, and an allowed redirect domain.
- Registered client data is persisted in a database (not hardcoded config).
- Attempting to use a client id that isn't registered is rejected.
- Attempting to redirect to a URI whose host doesn't match the client's registered redirect domain is rejected.

## Notes
- This story only covers storage/validation of client app registrations. It does not cover the login flow or the admin UI (sa-01a).
- Client id is a system-generated Guid (consistent with `User.UserId`), not a developer-chosen string.
- `ClientApp` has a single `RedirectDomain` field (one domain per client app). A redirect URI is valid if its host matches this domain — no separate redirect-URI table.
- Client secret is hashed using ASP.NET Core Identity's `PasswordHasher<T>`, never stored in plaintext.
- Validation logic lives in an App-layer service (`ClientRegistryService` or similar), backed directly by `ModelDbContext`. It exposes a single method returning an enum result (e.g. `Valid` / `UnknownClient` / `RedirectUriNotAllowed`) rather than separate bool checks or exceptions.
- `ClientApp` entity also carries `Name`, `CreatedBy` (FK to `User`), and `CreatedOn`.
- No admin UI yet, so registration for now is via a Development-only startup seeder: seeds a `system` `User` and one client app with a redirect URI on localhost. No seeding occurs outside Development.

## Open Questions
- none
