# Story: Client App Registry

- Status: not started
- Dependency: none

## Description
Identity needs to know which client apps are allowed to use it for SSO. Each client app is registered with a client id, client secret, and one or more allowed redirect URIs. This registry is the foundation the login and token flows check against.

## Acceptance Criteria
- A client app can be registered with: client id, client secret, and a list of allowed redirect URIs.
- Registered client data is persisted in a database (not hardcoded config).
- Attempting to use a client id that isn't registered is rejected.
- Attempting to redirect to a URI not in the client's registered list is rejected.

## Notes
- This story only covers storage/validation of client app registrations. It does not cover the login flow or the admin UI (sa-01a).

## Open Questions
- none
