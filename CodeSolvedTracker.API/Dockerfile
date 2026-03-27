FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["CodeSolvedTracker.API/CodeSolvedTracker.API.csproj", "CodeSolvedTracker.API/"]
COPY ["CodeSolvedTracker.Core/CodeSolvedTracker.Core.csproj", "CodeSolvedTracker.Core/"]
COPY ["CodeSolvedTracker.Infrastructure/CodeSolvedTracker.Infrastructure.csproj", "CodeSolvedTracker.Infrastructure/"]
COPY ["CodeSolvedTracker.DataCollector/CodeSolvedTracker.DataCollector.csproj", "CodeSolvedTracker.DataCollector/"]

# Restore
RUN dotnet restore "CodeSolvedTracker.API/CodeSolvedTracker.API.csproj"

# Copy all source code
COPY . .

# Publish API
WORKDIR "/src/CodeSolvedTracker.API"
RUN dotnet publish "CodeSolvedTracker.API.csproj" -c Release -o /app/publish

# Publish DataCollector
WORKDIR "/src/CodeSolvedTracker.DataCollector"
RUN dotnet publish "CodeSolvedTracker.DataCollector.csproj" -c Release -o /app/datacollector

# Create runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create data directory
RUN mkdir -p /app/data

# Copy published apps
COPY --from=build /app/publish ./api
COPY --from=build /app/datacollector ./datacollector

# Set working directory for seed
WORKDIR /app/datacollector

# Run DataCollector to seed the database
RUN dotnet CodeSolvedTracker.DataCollector.dll || true

# Switch back to API directory
WORKDIR /app/api

# Start the API
CMD ["dotnet", "CodeSolvedTracker.API.dll"]
