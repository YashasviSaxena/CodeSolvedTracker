FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["CodeSolvedTracker.API/CodeSolvedTracker.API.csproj", "CodeSolvedTracker.API/"]
COPY ["CodeSolvedTracker.Core/CodeSolvedTracker.Core.csproj", "CodeSolvedTracker.Core/"]
COPY ["CodeSolvedTracker.Infrastructure/CodeSolvedTracker.Infrastructure.csproj", "CodeSolvedTracker.Infrastructure/"]
COPY ["CodeSolvedTracker.DataCollector/CodeSolvedTracker.DataCollector.csproj", "CodeSolvedTracker.DataCollector/"]

# Restore all projects
RUN dotnet restore "CodeSolvedTracker.API/CodeSolvedTracker.API.csproj"
RUN dotnet restore "CodeSolvedTracker.DataCollector/CodeSolvedTracker.DataCollector.csproj"

# Copy all source code
COPY . .

# Publish API
WORKDIR "/src/CodeSolvedTracker.API"
RUN dotnet publish "CodeSolvedTracker.API.csproj" -c Release -o /app/api --no-restore

# Publish DataCollector
WORKDIR "/src/CodeSolvedTracker.DataCollector"
RUN dotnet publish "CodeSolvedTracker.DataCollector.csproj" -c Release -o /app/datacollector --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create data directory
RUN mkdir -p /app/data

# Copy published apps
COPY --from=build /app/api ./api
COPY --from=build /app/datacollector ./datacollector

# Create startup script
RUN echo '#!/bin/bash\n\
echo "=== Starting Code Solved Tracker ===\n\
\n\
# Run DataCollector to seed database\n\
cd /app/datacollector\n\
dotnet CodeSolvedTracker.DataCollector.dll\n\
\n\
# Start API\n\
cd /app/api\n\
dotnet CodeSolvedTracker.API.dll\n\
' > /app/start.sh && chmod +x /app/start.sh

# Start the application
CMD ["/app/start.sh"]
