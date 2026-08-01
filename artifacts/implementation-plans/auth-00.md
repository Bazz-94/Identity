# Plan for Implementing Story: auth-00

Story: `artifacts/stories/auth-00-admin-login-and-roles.md`

## Tasks
1. Domain: UserRole enum + User model refactor
  - Description: Add `Domain/Enums/UserRole.cs` (`Admin`, `Operator`, `User`). Refactor `User` to constructor-based creation (private setters, `UserId` generated via `Guid.NewGuid()` in the constructor, matching `ClientApp`'s pattern), adding `Email` and `Role` properties.
  - Acceptance Criteria: `User` compiles with no framework dependencies; constructing a `User` assigns a non-empty `UserId`, `UserName`, `Email`, `Role`. Existing callers (`ClientRegistryServiceTests`, `DevelopmentSeeder`) updated to use the new constructor.
  - Status: Completed

2. Infrastructure: EF migration for Email/Role
  - Description: Generate migration adding `Email` and `Role` columns to the `Users` table.
  - Acceptance Criteria: `dotnet ef migrations add AddUserEmailAndRole -p Infrastructure -s Api -o Database/Migrations` succeeds; `dotnet build Identity.slnx` succeeds.
  - Status: Completed

3. App: user lookup service
  - Description: Add `App/Services/AuthService.cs` with `FindUserByEmailAsync(string email) : Task<User?>`, backed by `ModelDbContext`.
  - Acceptance Criteria: Known email returns the matching `User`; unknown email returns `null`. Register in `AppExtensions.AddApp()`.
  - Status: In Progress

4. Api: cookie authentication + role policy wiring
  - Description: In `ApiExtensions.AddApi()`/`Program.cs`, add cookie authentication (`AddAuthentication().AddCookie(...)`) with `LoginPath` = the dev login page route and `AccessDeniedPath` = the Access Denied page route; add `app.UseAuthentication()` before `app.UseAuthorization()`.
  - Acceptance Criteria: App builds and runs; an unauthenticated request to a `[Authorize]`-protected route redirects to the login path.
  - Status: Not Started

5. Api: AuthController (login/logout)
  - Description: Add `Api/Controllers/AuthController.cs` with `POST /api/auth/login` (accepts email, looks up via `AuthService`; on match builds a `ClaimsPrincipal` with `NameIdentifier` = `UserId` and a `Role` claim, then `SignInAsync` the cookie scheme; on no match, fails the request) and `POST /api/auth/logout` (`SignOutAsync`).
  - Acceptance Criteria: Login with a seeded email issues the auth cookie; login with an unknown email creates no session; logout clears the session.
  - Status: Not Started

6. Client: dev login page + Access Denied page
  - Description: Add `Client/Pages/Login.cshtml`(+`.cs`) with an email input form posting to `/api/auth/login`, redirecting on success and showing an error on failure; add `Client/Pages/AccessDenied.cshtml`.
  - Acceptance Criteria: Submitting a seeded email logs in and redirects; an unknown email shows an error and no session is created; a role-restricted page hit by a wrong-role authenticated user redirects to `AccessDenied`.
  - Status: Not Started

7. Api: sample role-restricted endpoint for verification
  - Description: Apply `[Authorize(Roles = "Admin,Operator")]` to `WeatherForecastController` as the smoke-test artifact for role gating, until `sa-01` protects real client-registry endpoints/pages.
  - Acceptance Criteria: Unauthenticated request redirects to login; authenticated `User`-role request redirects to Access Denied; authenticated `Admin`/`Operator` request succeeds.
  - Status: Not Started

8. App: seed data update
  - Description: Update `DevelopmentSeeder` — existing seeded `system` user gets `Email` and `Role = Admin`; add a second seeded user with `Role = Operator`.
  - Acceptance Criteria: Running in Development seeds both users with correct `Email`/`Role`; seeding stays idempotent and non-Development environments still seed nothing.
  - Status: Not Started

9. Update story status
  - Description: Mark `auth-00-admin-login-and-roles.md` status as implemented once tasks 1-8 are verified.
  - Acceptance Criteria: Story file status field updated.
  - Status: Not Started

## Excludes
- Real Google OAuth integration (`sa-02`).
- Password/credential hashing for local login.
- Client registry UI/endpoints (`sa-01`) — this story only builds the shared auth foundation and a temporary demonstration endpoint.
