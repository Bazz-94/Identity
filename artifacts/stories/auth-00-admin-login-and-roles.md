# Story: Admin/Operator Login and Role-Gated Access

- Status: not started
- Dependency: sa-00-client-registry

## Description
Identity needs its own login for internal admin/operator access, separate from the SSO flow Identity provides to client apps. Users are global — the same `User` table and login mechanism back both this internal access and future external login (Google SSO), so this is not duplicated per app.

Real Google SSO is deferred to `sa-02`. This story stubs the login: a dev login page lets you pick a seeded user by email and signs in directly, establishing the same cookie session and Role claims that the real Google OAuth flow will produce later.

## Acceptance Criteria
- A dev login page (Client Razor Pages) lists/accepts an email and, on submit, posts to an Api `AuthController` endpoint.
- The endpoint looks up `User` by `Email` via an App-layer service; unknown email fails the login (no session created).
- On match, a cookie-based session is established carrying the user's id and Role as claims.
- `User` has `Email` (string) and `Role` (enum: Admin, Operator, User).
- Pages/endpoints can be restricted to a required role via an authorization policy (`[Authorize(Roles = ...)]`).
- An authenticated user without the required role hitting a restricted page is redirected to a custom Access Denied page.
- A logout endpoint clears the session.

## Notes
- This story is the shared authentication foundation. `sa-02` (Google SSO) replaces only the login endpoint's credential check with a real Google OAuth callback — the cookie sign-in, claims, and role-gating built here stay as-is.
- No password/credential hashing in this story — deferred along with real Google SSO.
- Client (UI) never calls App-layer services directly, per updated `.claude/rules/standards.md` — the dev login page posts to the Api controller, which calls the App service.
- Development seed data: existing seeded `system` User gets `Email` + `Role = Admin`; a second seeded user is added with `Role = Operator`, to exercise role gating.

## Open Questions
- none
