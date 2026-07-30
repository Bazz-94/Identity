# Story: Google SSO Login

- Status: not started
- Dependency: sa-01-client-registry

## Description
A client app redirects a user to Identity's login page to authenticate. Identity validates the requesting client and redirect URI against the registry, sends the user through Google sign-in, and on success redirects back to the client app with an authorization code. If this is the user's first time signing in via Google, a local user record is created automatically.

## Acceptance Criteria
- Identity exposes a login endpoint/page that accepts a client id and redirect URI.
- The client id and redirect URI are validated against the client registry (sa-01) before proceeding.
- The user is sent through Google's OIDC sign-in flow.
- On successful Google authentication, Identity looks up the local user by Google `sub` (subject id).
- If no local user exists for that Google `sub`, one is created automatically using profile info from Google.
- On success, Identity redirects back to the client app's redirect URI with an authorization code.
- Invalid client id or redirect URI results in a rejected request (no redirect to an unregistered URI).
- The Postman collection is updated with requests to exercise the login flow (as far as it can be scripted outside a browser-based Google consent screen).

## Notes
- Only Google is supported as an identity provider for now. No local username/password login.
- This story covers the authentication + authorization code issuance step, not the code-to-token exchange (sa-03).

## Open Questions
- What profile fields from Google are stored on the local user record (email, name, picture, etc.)?
