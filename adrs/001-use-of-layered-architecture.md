# ADR 001: Use of Layered Architecture

## Status
Accepted

## Context
The project requires a clean separation of concerns to promote maintainability, testability, and scalability. A layered architecture is a common approach to achieve these goals.

## Decision
We decided to use a layered architecture with the following layers:
- **API Layer**: Handles HTTP requests and responses.
- **Application Layer**: Contains the core business logic and use cases.
- **Domain Layer**: Defines the core entities, value objects, and interfaces.
- **Persistence Layer**: Manages data access using Entity Framework Core.

## Consequences
- **Pros**:
  - Clear separation of concerns.
  - Improved testability and maintainability.
  - Flexibility to change implementations without affecting other layers.
- **Cons**:
  - Increased complexity due to multiple layers.
  - Potential performance overhead due to additional abstraction layers.
