# Story: Client Registry Management UI

- Status: not started
- Dependency: sa-01-client-registry

## Description
Admins manage client app registrations through a dedicated page — registering new clients and viewing/editing/deleting existing ones.

## Acceptance Criteria
- Only authenticated admins/operators can access the client registry management page.
- An admin can register a new client app through the page; the client secret is generated and shown once at creation.
- An admin can view a list of all registered client apps and their details (raw secret is not shown again after creation).
- An admin can edit an existing client app's redirect URIs from the page.
- An admin can delete a client app from the page.

## Notes
- The Postman collection is updated with requests to exercise the client registry endpoints (create/list/edit/delete client apps).

## Open Questions
- If a client secret is lost, is regenerating it the only recovery path (invalidating the old one), or is there another flow?
