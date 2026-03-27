FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["CodeSolvedTracker.API/CodeSolvedTracker.API.csproj", "CodeSolvedTracker.API/"]
COPY ["CodeSolvedTracker.Core/CodeSolvedTracker.Core.csproj", "CodeSolvedTracker.Core/"]
COPY ["CodeSolvedTracker.Infrastructure/CodeSolvedTracker.Infrastructure.csproj", "CodeSolvedTracker.Infrastructure/"]
COPY ["CodeSolvedTracker.DataCollector/CodeSolvedTracker.DataCollector.csproj", "CodeSolvedTracker.DataCollector/"]

# Restore
RUN dotnet restore "CodeSolvedTracker.API/CodeSolvedTracker.API.csproj"

# Copy all source
COPY . .

# Publish API
WORKDIR "/src/CodeSolvedTracker.API"
RUN dotnet publish "CodeSolvedTracker.API.csproj" -c Release -o /app/publish

# Publish DataCollector
WORKDIR "/src/CodeSolvedTracker.DataCollector"
RUN dotnet publish "CodeSolvedTracker.DataCollector.csproj" -c Release -o /app/datacollector

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy both published apps
COPY --from=build /app/publish ./api
COPY --from=build /app/datacollector ./datacollector

# Copy seed script
COPY seed-database.sh .

# Make script executable
RUN chmod +x seed-database.sh

# Run seed script then start API
CMD ./seed-database.sh && cd api && dotnet CodeSolvedTracker.API.dll
