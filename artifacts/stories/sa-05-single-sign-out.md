# Story: Single Sign-Out

- Status: not started
- Dependency: sa-03-token-issuance

## Description
A user can log out at Identity, ending their authenticated session across all client apps that checked in during the SSO flow, not just the one they logged out from.

## Acceptance Criteria
- Identity exposes a logout endpoint.
- Logging out invalidates the user's outstanding refresh token(s).
- Client apps that participated in the session are notified/able to detect the logout (mechanism to be defined during implementation, e.g. OIDC back-channel logout).
- The Postman collection is updated with a request to trigger logout.

## Notes
- Identity is stateless with respect to sessions (no server-side session cookie per earlier decision), so "single sign-out" here means revoking tokens/refresh tokens rather than clearing a shared session.

## Open Questions
- Since Identity holds no session, how do client apps get notified of logout — back-channel logout call to each registered client, or do client apps just find out on next token refresh attempt (which will fail)?
