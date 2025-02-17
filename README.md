# TradeManagementApp

## TL;DR

1. **Clone the Repository**:
    ```sh
    git clone https://github.com/ujjavala/trade-management-app
    cd TradeManagementApp
    ```

2. **Ensure All Projects Target .NET 6.0**:
    Make sure all your project files (.csproj) are targeting net6.0.

3. **Clean and Restore the Solution**:
    ```sh
    dotnet clean
    dotnet restore
    ```

4. **Build the Solution**:
    ```sh
    dotnet build
    ```

5. **Setup SQLite Database**:
    ```sh
    cd TradeManagementApp/TradeManagementApp.API
    dotnet ef migrations add InitialCreate --project ../TradeManagementApp.Persistence/TradeManagementApp.Persistence.csproj
    dotnet ef database update --project ../TradeManagementApp.Persistence/TradeManagementApp.Persistence.csproj
    ```

6. **Run the Application**:
    ```sh
    dotnet run --project TradeManagementApp.API
    ```

7. **Run the Unit Tests**:
    ```sh
    dotnet test
    ```

8. **Build and Run with Docker**:
    ```sh
    docker-compose build
    docker-compose up
    ```

## Overview

TradeManagementApp is a sample application designed to manage trades and accounts. It demonstrates a layered architecture to promote separation of concerns, testability, and maintainability. The application consists of the following projects:

- `TradeManagementApp.API`: The ASP.NET Core API project responsible for handling HTTP requests and responses. It orchestrates the application flow but contains minimal business logic.
- `TradeManagementApp.Application`: The application layer containing the core business logic and use cases. It depends on the Domain layer and defines the application's services.
- `TradeManagementApp.Domain`: The domain layer containing the core entities, value objects, and interfaces that define the business domain. It is independent of any specific technology or framework.
- `TradeManagementApp.Persistence`: The persistence layer responsible for data access. It implements the repository interfaces defined in the Domain layer using Entity Framework Core.
- `TradeManagementApp.Tests`: The unit and integration tests for the application.

**Why this Architecture?**

This layered architecture is designed to achieve the following benefits:

*   **Separation of Concerns:** Each layer has a specific responsibility, making the code easier to understand, maintain, and test.
*   **Testability:** The domain layer is easily testable because it has no dependencies on infrastructure concerns. The application layer can be tested in isolation using mock repositories.
*   **Maintainability:** Changes in one layer are less likely to affect other layers. For example, you can change the database implementation without affecting the application or domain logic.
*   **Reusability:** The domain layer can be reused in other applications or services.
*   **Flexibility:** You can easily switch between different infrastructure implementations (e.g., different databases, different API frameworks) without affecting the core business logic.

## Prerequisites

-   [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
-   [Visual Studio Code](https://code.visualstudio.com/) or any other IDE of your choice.
-   [Docker](https://www.docker.com/products/docker-desktop)

## Setup

### Step 1: Clone the Repository

```sh
git clone https://github.com/ujjavala/trade-management-app
cd TradeManagementApp
```

### Step 2: Ensure All Projects Target .NET 6.0

Make sure all your project files (.csproj) are targeting net6.0.

### Step 3: Clean and Restore the Solution

Run the following commands to clean and restore the solution:

```sh
dotnet clean
dotnet restore
```

### Step 4: Build the Solution

Run the following command to build the solution:

```sh
dotnet build
```

### Step 5: Setup SQLite Database

Navigate to the TradeManagementApp.API directory and add seed data to the database by running the following commands:

```sh
cd TradeManagementApp/TradeManagementApp.API
dotnet ef migrations add InitialCreate --project ../TradeManagementApp.Persistence/TradeManagementApp.Persistence.csproj
dotnet ef database update --project ../TradeManagementApp.Persistence/TradeManagementApp.Persistence.csproj
```

### Step 6: Configure Swagger

Swagger is used for API documentation and testing. It is already configured in the `Startup.cs` file of the `TradeManagementApp.API` project. To access the Swagger UI, run the application and navigate to `http://localhost:5294/swagger`.

### Step 7: Run the Application

Run the application using the following command:

```sh
dotnet run --project TradeManagementApp.API
```

By default, the application will run on `http://localhost:5294`.

### Step 8: Run the Unit Tests

Run the unit tests using the following command:

```sh
dotnet test
```

### Endpoints

The API provides the following endpoints:

#### Accounts

-   GET /api/accounts: Retrieves all accounts.
-   GET /api/accounts/{id}: Retrieves a specific account by ID.
-   POST /api/accounts: Creates a new account.
-   PUT /api/accounts/{id}: Updates an existing account by ID. If no ID is supplied in the body, the ID from the URL is used. If the IDs do not match, a `BadRequest` is returned.
-   DELETE /api/accounts/{id}: Deletes an account by ID.
-   GET /api/accounts/search?id={id}&lastName={lastName}: Searches for accounts by ID and/or last name.

#### Trades

-   GET /api/trades: Retrieves all trades.
-   GET /api/trades/{id}: Retrieves a specific trade by ID.
-   POST /api/trades: Creates a new trade.
-   PUT /api/trades/{id}: Updates an existing trade by ID. If no ID is supplied in the body, the ID from the URL is used. If the IDs do not match, a `BadRequest` is returned.
-   DELETE /api/trades/{id}: Deletes a trade by ID.

### Code Explanation

#### TradeManagementApp.API

-   **Controllers**: Contains the API controllers that handle HTTP requests and return responses. The controllers orchestrate the application flow by calling the appropriate methods in the Application layer.
    -   **AccountsController**: Manages account-related operations, including searching for accounts.
    -   **TradesController**: Manages trade-related operations.
-   **Services**: *[Optional]* In more complex scenarios, this layer might contain API-specific service implementations for tasks like request validation, data transformation (using DTOs), and handling API-specific concerns. In this simplified example, the controllers directly interact with the Application layer services.

#### TradeManagementApp.Application

-   **Services**: Contains the application service interfaces (e.g., `IAccountService`, `ITradeService`) and implementations. These services encapsulate the core business logic and use cases of the application. They depend on the Domain layer for entities and repository interfaces.

#### TradeManagementApp.Domain

-   **Models**: Defines the core domain entities (e.g., `Account`, `Trade`, `TradeStatus`) that represent the business concepts.
    -   `TradeStatus`: An enum representing the status of a trade (Placed, Executed, Expired). Serialized as strings in the API.
-   **Repositories**: Defines the repository interfaces (e.g., `IAccountRepository`, `ITradeRepository`) that abstract the data access logic.

#### TradeManagementApp.Persistence

-   **DataContext**: The Entity Framework Core `DbContext` that manages the database connection and entity sets.
-   **Repositories**: Contains the repository classes that implement the repository interfaces defined in the Domain layer. These classes handle the data access logic using Entity Framework Core.
    -   **AccountRepository**: Implements the `IAccountRepository` interface.
    -   **TradeRepository**: Implements the `ITradeRepository` interface.

#### TradeManagementApp.Tests

Contains unit and integration tests for the application to ensure the correctness of the business logic and data access.

-   **Controllers**
    -   **AccountsControllerTests**: Tests for the `AccountsController`.
    -   **TradesControllerTests**: Tests for the `TradesController`.
-   **Services**
    -   **AccountServiceTests**: Tests for the `AccountService`.
    -   **TradeServiceTests**: Tests for the `TradeService`.

### Orchestration Explanation

The TradeManagementApp application follows a layered architecture, where each layer has a specific responsibility. Here is a detailed explanation of how the different components interact with each other:

-   **Controllers**: The controllers are the entry points for the API. They receive HTTP requests from clients and delegate the processing to the appropriate service methods in the Application layer. The controllers are responsible for returning the appropriate HTTP responses.
-   **Services (Application Layer)**: The services in the Application layer contain the business logic of the application. They implement the interfaces defined in the Application layer and interact with the repositories to perform CRUD operations. The services are injected into the controllers via dependency injection.
-   **Repositories**: The repositories are responsible for data access. They implement the interfaces defined in the Domain layer and interact with the `DataContext` to perform database operations. The repositories are injected into the services via dependency injection.
-   **DataContext**: The `DataContext` is the Entity Framework Core `DbContext` that manages the database connection and entity sets. It is configured in the `Startup` class and used by the repositories to interact with the database.
-   **Models**: The models define the data structures used in the application. They are defined in the Domain layer and used by the `DataContext` to create the database schema and by the services and controllers to transfer data.
-   **Unit Tests**: The unit tests ensure the correctness of the business logic and data access. They use the Moq library to create mock objects for the dependencies and the xUnit framework to define and run the tests.

#### Example Flow

1.  A client sends a POST request to `/api/accounts` to create a new account.
2.  The `AccountsController` receives the request and calls the `AddAccountAsync` method of the `AccountService` in the Application layer.
3.  The `AccountService` calls the `AddAccountAsync` method of the `AccountRepository`.
4.  The `AccountRepository` uses the `DataContext` to add the new account to the database.
5.  The `AccountRepository` returns control to the `AccountService`, which returns control to the `AccountsController`.
6.  The `AccountsController` returns a `CreatedAtActionResult` response to the client.

This orchestration ensures a clean separation of concerns and makes the application easy to maintain and extend.

### Caching Implementation

To improve performance, the application implements a caching mechanism for retrieving account data. The `AccountService` utilizes an LRU (Least Recently Used) cache to cache account information.

#### Caching Strategy

The caching strategy involves the following steps:

1.  **Check the Cache:** When retrieving an account by ID, the `AccountService` first checks if the account exists in the LRU cache using a cache key (e.g., `"account-{accountId}"`).
2.  **Return Cached Data (if available):** If the account is found in the cache, it is returned directly, bypassing the database query.
3.  **Retrieve from Database (if not in cache):** If the account is not found in the cache, the `AccountService` retrieves it from the database using the `AccountRepository`.
4.  **Add to Cache:** After retrieving the account from the database, the `AccountService` adds it to the cache with an appropriate cache key. The LRU cache ensures that the least recently used items are evicted when the cache reaches its capacity.

#### Code Snippet

```csharp
public async Task<Account> GetAccountByIdWithCacheAsync(int accountId)
{
    string cacheKey = $"account-{accountId}";

    if (_lruCache.TryGetValue(cacheKey, out Account account))
    {
        return account; // Return cached account
    }

    account = await _accountRepository.GetAccountByIdAsync(accountId);

    if (account != null)
    {
        _lruCache.Set(cacheKey, account); // Add to LRU cache
    }

    return account;
}
```

#### Benefits of Caching

-   **Reduced Database Load:** Caching reduces the number of database queries, which can significantly improve performance, especially for frequently accessed data.
-   **Improved Response Times:** Serving data from the cache is much faster than retrieving it from the database, resulting in faster response times for API requests.
-   **Scalability:** Caching can help improve the scalability of the application by reducing the load on the database server.

### Docker Configuration

The application is configured to run in a Docker container. The `Dockerfile` includes the following configurations:

-   **Base Image**: Uses the official .NET SDK and ASP.NET runtime images.
-   **Copying Source Code**: Copies the source code into the container.
-   **Restoring Dependencies**: Restores the project dependencies using `dotnet restore`.
-   **Building the Application**: Builds the application using `dotnet build`.
-   **Publishing the Application**: Publishes the application using `dotnet publish`.
-   **Setting Environment Variables**: Sets the `ASPNETCORE_URLS` environment variable to configure the application to listen on all network interfaces.
-   **Copying the SQLite Database**: Copies the SQLite database file into the container.
-   **Setting the Connection String**: Configures the connection string using the `ConnectionStrings__DefaultConnection` environment variable.
-   **Entry Point**: Specifies the entry point for the application using `dotnet TradeManagementApp.API.dll`.

The `docker-compose.yml` file defines the services, networks, and volumes used by the application.

### Additional Information

-   Ensure that you have the correct version of the .NET SDK installed.
-   If you encounter any issues, please check the error messages and ensure that all dependencies are correctly installed.

### Architecture Decision Records (ADRs)

The following ADRs document the architectural decisions made for this project:

1. **[ADR 001: Use of Layered Architecture](adrs/001-use-of-layered-architecture.md)**
    - Decision to use a layered architecture to promote separation of concerns, testability, and maintainability.
2. **[ADR 002: Use of Entity Framework Core](adrs/002-use-of-entity-framework-core.md)**
    - Decision to use Entity Framework Core for data access in the Persistence Layer.
3. **[ADR 003: Use of In-Memory Caching with LRU Strategy](adrs/003-use-of-in-memory-caching-with-lru-strategy.md)**
    - Decision to use in-memory caching with an LRU strategy for caching account data.
4. **[ADR 004: Use of Docker for Containerization](adrs/004-use-of-docker-for-containerization.md)**
    - Decision to use Docker for containerizing the application and its dependencies.
5. **[ADR 005: Use of Swagger for API Documentation](adrs/005-use-of-swagger-for-api-documentation.md)**
    - Decision to use Swagger for API documentation.
6. **[ADR 006: Use of xUnit for Unit Testing](adrs/006-use-of-xunit-for-unit-testing.md)**
    - Decision to use xUnit for unit testing.
7. **[ADR 007: Use of Moq for Mocking Dependencies](adrs/007-use-of-moq-for-mocking-dependencies.md)**
    - Decision to use Moq for mocking dependencies in unit tests.
8. **[ADR 008: Use of SQLite for Development and Testing](adrs/008-use-of-sqlite-for-development-and-testing.md)**
    - Decision to use SQLite as the database for development and testing environments.
9. **[ADR 009: Use of Redis for Distributed Caching](adrs/009-use-of-redis-for-distributed-caching.md)**
    - Decision to use Redis for distributed caching in the production environment.
10. **[ADR 010: Implement Authentication Layer](adrs/010-implement-authentication-layer.md)**
    - Decision to implement an authentication layer to secure the API endpoints.
11. **[ADR 011: Implement ORM for Handling Complex DTO Models](adrs/011-implement-orm-for-handling-complex-dto-models.md)**
    - Decision to implement an ORM using Entity Framework Core to handle complex DTO models.
12. **[ADR 012: Fix Integration Tests](adrs/012-fix-integration-tests.md)**
    - Decision to review and fix the integration tests to ensure they work correctly with the new architecture.
13. **[ADR 013: Use of LRU Cache for In-Memory Caching](adrs/013-use-of-lru-cache-for-in-memory-caching.md)**
    - Decision to use in-memory caching with an LRU strategy for caching account data.

### TODO

The following tasks are still pending:

-   Fix Integration Tests: The integration tests that are failing due to platform dependencies need to be reviewed and fixed to ensure they are working correctly with the new architecture.
-   Add Authentication Layer: Implement an authentication layer to secure the API endpoints.
-   For achieving scalability and efficiency on load balancer front for Cache, Redis can be used instead of in-memory caching.
-   Implement ORM for handling complex DTO models: Use an Object-Relational Mapping (ORM) tool like Entity Framework Core to manage complex Data Transfer Objects (DTOs) and their relationships. This will simplify data access and manipulation, and ensure consistency across the application.