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

# Copy the remaining files and build the application
COPY . .
RUN dotnet build -c Release

# Publish the application
RUN dotnet publish TradeManagementApp.API/TradeManagementApp.API.csproj -c Release -o /app/publish

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TradeManagementApp.API.dll"]