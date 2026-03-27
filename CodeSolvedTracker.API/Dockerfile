FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["CodeSolvedTracker.API/CodeSolvedTracker.API.csproj", "CodeSolvedTracker.API/"]
COPY ["CodeSolvedTracker.Core/CodeSolvedTracker.Core.csproj", "CodeSolvedTracker.Core/"]
COPY ["CodeSolvedTracker.Infrastructure/CodeSolvedTracker.Infrastructure.csproj", "CodeSolvedTracker.Infrastructure/"]
COPY ["CodeSolvedTracker.DataCollector/CodeSolvedTracker.DataCollector.csproj", "CodeSolvedTracker.DataCollector/"]

RUN dotnet restore "CodeSolvedTracker.API/CodeSolvedTracker.API.csproj"

COPY . .

# Publish API
WORKDIR "/src/CodeSolvedTracker.API"
RUN dotnet publish "CodeSolvedTracker.API.csproj" -c Release -o /app/api

# Publish DataCollector
WORKDIR "/src/CodeSolvedTracker.DataCollector"
RUN dotnet publish "CodeSolvedTracker.DataCollector.csproj" -c Release -o /app/datacollector

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Install SQLite command line tools
RUN apt-get update && apt-get install -y sqlite3

# Create data directory
RUN mkdir -p /app/data

# Copy published apps
COPY --from=build /app/api ./api
COPY --from=build /app/datacollector ./datacollector

# Copy startup script
COPY startup.sh .
RUN chmod +x startup.sh

# Run startup script
CMD ["./startup.sh"]
