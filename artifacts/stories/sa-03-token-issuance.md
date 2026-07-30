# Story: Token Issuance

- Status: not started
- Dependency: sa-02-google-sso-login

## Description
A client app exchanges the authorization code it received from the login flow for a JWT ID token and access token. This is the standard OIDC authorization code exchange step, and is the point at which the client app receives a usable token for the authenticated user.

## Acceptance Criteria
- Identity exposes a token endpoint that accepts an authorization code, client id, and client secret.
- The authorization code is validated (belongs to the client, not expired, not already used).
- On success, Identity returns a signed JWT ID token and access token.
- The ID token contains standard OIDC claims plus the local user identity.
- An invalid, expired, or already-used authorization code is rejected.
- Invalid client credentials are rejected.
- The Postman collection is updated with a request to exchange an authorization code for tokens.

## Notes
- No server-side session is kept; Identity is stateless. Client apps are responsible for storing/using the JWT however they choose.

## Open Questions
- What is the access token/ID token lifetime?
