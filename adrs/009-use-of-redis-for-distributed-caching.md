# ADR 009: Use of Redis for Distributed Caching

## Status
Pending

## Context
To achieve scalability and efficiency on the load balancer front for cache, a distributed caching solution is required. Redis is a popular choice for distributed caching.

## Decision
We are considering using Redis for distributed caching in the production environment.

## Consequences
- **Pros**:
  - Provides a scalable and efficient caching solution.
  - Supports advanced caching features like expiration policies and pub/sub.
  - Integrates well with .NET applications.
- **Cons**:
  - Requires additional infrastructure and configuration.
  - Potential learning curve for developers unfamiliar with Redis.
