# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the solution file
COPY *.sln .

# Copy the project files and restore dependencies
COPY TradeManagementApp.API/*.csproj TradeManagementApp.API/
COPY TradeManagementApp.Application/*.csproj TradeManagementApp.Application/
COPY TradeManagementApp.Domain/*.csproj TradeManagementApp.Domain/
COPY TradeManagementApp.Persistence/*.csproj TradeManagementApp.Persistence/
COPY TradeManagementApp.Tests/*.csproj TradeManagementApp.Tests/
RUN dotnet restore

# Copy the database file into the build stage
COPY TradeManagementApp.Persistence/trade_management.db /app/trade_management.db

# Copy the remaining files and build the application
COPY . .
RUN dotnet build -c Release

# Publish the application
RUN dotnet publish TradeManagementApp.API/TradeManagementApp.API.csproj -c Release -o /app/publish

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS api
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Copy the database file from the build stage
COPY --from=build /app/trade_management.db /app/trade_management.db

# Ensure the database file is copied and has correct permissions
RUN chmod +rwx /app/trade_management.db

# Set the connection string using an environment variable
ENV ConnectionStrings__DefaultConnection="Data Source=/app/trade_management.db"

ENTRYPOINT ["dotnet", "TradeManagementApp.API.dll"]