# Story: Client Registry Management UI

- Status: implemented
- Dependency: sa-00-client-registry, auth-00-admin-login-and-roles

## Description
Admins manage client app registrations through a dedicated page — registering new clients and viewing/editing/deleting existing ones.

## Acceptance Criteria
- Only authenticated admins/operators can access the client registry management page.
- An admin can register a new client app through the page; the client secret is generated and shown once at creation.
- An admin can view a list of all registered client apps and their details (raw secret is not shown again after creation).
- An admin can edit an existing client app's redirect URIs from the page.
- An admin can delete a client app from the page.

## Notes
- The Postman collection is updated with requests to exercise the client registry endpoints (create/list/edit/delete client apps). No collection exists in the repo yet — this story creates one.
- Role scope: Admin has full access (create/edit/delete); Operator is read-only (can view the page/list but not create/edit/delete).
- Single page (`Client/Pages/ClientApps.cshtml`) with inline list, create form, and per-row edit/delete — same vanilla-JS `fetch()` pattern as `Login.cshtml`. Gated via `[Authorize(Roles = "Admin,Operator")]` on the `PageModel`; create/edit/delete controls are further restricted to Admin in the API layer (`[Authorize(Roles = "Admin")]` per action).
- New `Api/Controllers/ClientAppsController.cs`, REST routes under `api/client-apps`: `POST` (create, Admin), `GET` (list, Admin+Operator), `PUT {id}` (update Name + RedirectDomain, Admin), `DELETE {id}` (Admin).
- `ClientApp` keeps its single `RedirectDomain` field (no schema change) — "edit redirect URIs" means replacing this one value. Edit also allows renaming (`Name`).
- Plaintext client secret is generated as `Guid.NewGuid().ToString("N")`, hashed via the existing `ClientSecretHasher`, and returned once in the create response only.
- Delete is a hard delete (no soft-delete flag) — no other tables reference `ClientApp` today.
- List response includes resolved creator display info (`CreatedByUser.UserName`), not just the raw `CreatedBy` Guid.
- `Api.Tests/RoleAuthorizationTests.cs` (currently targets the deleted `WeatherForecastController`) is retargeted to the new client-registry endpoints as part of this story.

## Open Questions
- none (regenerate-secret recovery flow is explicitly out of scope/deferred to a future story)
