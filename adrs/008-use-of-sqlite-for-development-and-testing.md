# ADR 008: Use of SQLite for Development and Testing

## Status
Accepted

## Context
The project requires a lightweight and easy-to-configure database for development and testing. SQLite is a suitable choice for this purpose.

## Decision
We decided to use SQLite as the database for development and testing environments.

## Consequences
- **Pros**:
  - Lightweight and easy to configure.
  - No need for a separate database server.
  - Supports most features required for development and testing.
- **Cons**:
  - Limited scalability compared to other databases.
  - May require adjustments when switching to a different database in production.
