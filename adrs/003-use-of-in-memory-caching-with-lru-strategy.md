# ADR 003: Use of In-Memory Caching with LRU Strategy

## Status
Accepted

## Context
To improve performance and reduce database load, the project requires a caching mechanism. An LRU (Least Recently Used) cache is a suitable strategy for managing cache eviction.

## Decision
We decided to use in-memory caching with an LRU strategy for caching account data in the Application Layer.

## Consequences
- **Pros**:
  - Reduces database load and improves response times.
  - Simple implementation using in-memory data structures.
  - LRU strategy ensures that frequently accessed items remain in the cache.
- **Cons**:
  - Limited by available memory.
  - Cache data is lost if the application restarts.
  - May require additional configuration for cache size and eviction policies.
