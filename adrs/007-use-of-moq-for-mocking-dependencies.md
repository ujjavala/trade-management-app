# ADR 007: Use of Moq for Mocking Dependencies

## Status
Accepted

## Context
The project requires a mocking framework to isolate dependencies in unit tests. Moq is a widely used mocking framework for .NET applications.

## Decision
We decided to use Moq for mocking dependencies in the Tests project.

## Consequences
- **Pros**:
  - Simplifies the creation of mock objects for dependencies.
  - Supports a fluent API for configuring mock behavior.
  - Integrates well with xUnit and other testing frameworks.
- **Cons**:
  - Learning curve for developers unfamiliar with Moq.
  - Requires additional configuration for complex mock setups.
