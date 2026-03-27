FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["CodeSolvedTracker.API/CodeSolvedTracker.API.csproj", "CodeSolvedTracker.API/"]
COPY ["CodeSolvedTracker.Core/CodeSolvedTracker.Core.csproj", "CodeSolvedTracker.Core/"]
COPY ["CodeSolvedTracker.Infrastructure/CodeSolvedTracker.Infrastructure.csproj", "CodeSolvedTracker.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "CodeSolvedTracker.API/CodeSolvedTracker.API.csproj"

# Copy all source code
COPY . .

# Build and publish
WORKDIR "/src/CodeSolvedTracker.API"
RUN dotnet publish "CodeSolvedTracker.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CodeSolvedTracker.API.dll"]
