# Story: Refresh Token

- Status: not started
- Dependency: sa-03-token-issuance

## Description
A client app can obtain a new access token using a refresh token, without requiring the user to sign in again via Google.

## Acceptance Criteria
- Identity issues a refresh token alongside the access/ID token during token issuance (sa-03).
- Identity exposes a token endpoint mode (or equivalent) that accepts a refresh token and client credentials.
- A valid refresh token returns a new access token (and ID token).
- An invalid, expired, or revoked refresh token is rejected.
- The Postman collection is updated with a request to exchange a refresh token for a new access token.

## Notes
- Refresh token rotation policy (single-use vs reusable) to be decided during implementation.

## Open Questions
- Refresh token lifetime?
- Should refresh tokens rotate on use (old one invalidated, new one issued)?
