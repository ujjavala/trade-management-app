# ADR 013: Use of LRU Cache for In-Memory Caching

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

## Implementation

The caching strategy involves the following steps:

1. **Check the Cache:** When retrieving an account by ID, the `AccountService` first checks if the account exists in the LRU cache using a cache key (e.g., `"account-{accountId}"`).
2. **Return Cached Data (if available):** If the account is found in the cache, it is returned directly, bypassing the database query.
3. **Retrieve from Database (if not in cache):** If the account is not found in the cache, the `AccountService` retrieves it from the database using the `AccountRepository`.
4. **Add to Cache:** After retrieving the account from the database, the `AccountService` adds it to the cache with an appropriate cache key. The LRU cache ensures that the least recently used items are evicted when the cache reaches its capacity.