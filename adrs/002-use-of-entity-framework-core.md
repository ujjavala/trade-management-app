# ADR 002: Use of Entity Framework Core

## Status
Accepted

## Context
The project requires a robust and flexible ORM to manage database interactions. Entity Framework Core is a popular ORM for .NET applications.

## Decision
We decided to use Entity Framework Core for data access in the Persistence Layer.

## Consequences
- **Pros**:
  - Simplifies data access and manipulation.
  - Supports LINQ queries and migrations.
  - Integrates well with .NET applications.
- **Cons**:
  - Learning curve for developers unfamiliar with Entity Framework Core.
  - Potential performance overhead compared to raw SQL queries.
