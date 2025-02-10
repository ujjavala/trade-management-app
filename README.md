
# TradeManagementApp

## Overview

TradeManagementApp is a sample application designed to manage trades and accounts. It consists of multiple projects:
- `TradeManagementApp.API`: The main API project.
- `TradeManagementApp.Persistence`: The persistence layer for data access.
- `TradeManagementApp.Models`: The shared models used across the application.
- `TradeManagementApp.Tests`: The unit tests for the application.

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio Code](https://code.visualstudio.com/) or any other IDE of your choice.

## Setup

### Step 1: Clone the Repository

```sh
git clone <repository-url>
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

- GET /api/accounts: Retrieves all accounts.
- GET /api/accounts/{id}: Retrieves a specific account by ID.
- POST /api/accounts: Creates a new account.
- PUT /api/accounts/{id}: Updates an existing account by ID.
- DELETE /api/accounts/{id}: Deletes an account by ID.

#### Trades

- GET /api/trades: Retrieves all trades.
- GET /api/trades/{id}: Retrieves a specific trade by ID.
- POST /api/trades: Creates a new trade.
- PUT /api/trades/{id}: Updates an existing trade by ID.
- DELETE /api/trades/{id}: Deletes a trade by ID.

### Code Explanation

#### TradeManagementApp.API

- **Controllers**: Contains the API controllers that handle HTTP requests and return responses.
  - **AccountsController**: Manages account-related operations.
  - **TradesController**: Manages trade-related operations.
- **Services**: Contains the business logic for managing accounts and trades.
  - **AccountService**: Implements the IAccountService interface and contains methods for account operations.
  - **TradeService**: Implements the ITradeService interface and contains methods for trade operations.

#### TradeManagementApp.Persistence

- **DataContext**: The Entity Framework Core DbContext that manages the database connection and entity sets.
- **Repositories**: Contains the repository classes that handle data access logic for accounts and trades.
  - **IAccountRepository**: Interface defining methods for account data access.
  - **ITradeRepository**: Interface defining methods for trade data access.
  - **AccountRepository**: Implements the IAccountRepository interface.
  - **TradeRepository**: Implements the ITradeRepository interface.

#### TradeManagementApp.Models

- **Account.cs**: Defines the Account model with properties such as Id, FirstName, LastName, and Trades.
- **Trade.cs**: Defines the Trade model with properties such as Id, AccountId, SecurityCode, Timestamp, Amount, BuyOrSell, Status, and Account.

#### TradeManagementApp.Tests

Contains unit tests for the application to ensure the correctness of the business logic and data access.

- **Controllers**
  - **AccountsControllerTests**: Tests for the AccountsController.
  - **TradesControllerTests**: Tests for the TradesController.
- **Services**
  - **AccountServiceTests**: Tests for the AccountService.
  - **TradeServiceTests**: Tests for the TradeService.

### Orchestration Explanation

The TradeManagementApp application follows a layered architecture, where each layer has a specific responsibility. Here is a detailed explanation of how the different components interact with each other:

- **Controllers**: The controllers are the entry points for the API. They receive HTTP requests from clients and delegate the processing to the appropriate service methods. The controllers are responsible for returning the appropriate HTTP responses.
- **Services**: The services contain the business logic of the application. They implement the interfaces defined in the Services directory and interact with the repositories to perform CRUD operations. The services are injected into the controllers via dependency injection.
- **Repositories**: The repositories are responsible for data access. They implement the interfaces defined in the Repositories directory and interact with the DataContext to perform database operations. The repositories are injected into the services via dependency injection.
- **DataContext**: The DataContext is the Entity Framework Core DbContext that manages the database connection and entity sets. It is configured in the Startup class and used by the repositories to interact with the database.
- **Models**: The models define the data structures used in the application. They are used by the DataContext to create the database schema and by the services and controllers to transfer data.
- **Unit Tests**: The unit tests ensure the correctness of the business logic and data access. They use the Moq library to create mock objects for the dependencies and the xUnit framework to define and run the tests.

#### Example Flow

1. A client sends a POST request to /api/accounts to create a new account.
2. The AccountsController receives the request and calls the AddAccountAsync method of the AccountService.
3. The AccountService calls the AddAccountAsync method of the AccountRepository.
4. The AccountRepository uses the DataContext to add the new account to the database.
5. The AccountRepository returns control to the AccountService, which returns control to the AccountsController.
6. The AccountsController returns a CreatedAtActionResult response to the client.

This orchestration ensures a clean separation of concerns and makes the application easy to maintain and extend.

### Additional Information

- Ensure that you have the correct version of the .NET SDK installed.
- If you encounter any issues, please check the error messages and ensure that all dependencies are correctly installed.

This should provide a comprehensive guide to setting up, building, running, and testing the TradeManagementApp. If you encounter any further issues, please let me know!
