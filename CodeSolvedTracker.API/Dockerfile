FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["CodeSolvedTracker.API/CodeSolvedTracker.API.csproj", "CodeSolvedTracker.API/"]
COPY ["CodeSolvedTracker.Core/CodeSolvedTracker.Core.csproj", "CodeSolvedTracker.Core/"]
COPY ["CodeSolvedTracker.Infrastructure/CodeSolvedTracker.Infrastructure.csproj", "CodeSolvedTracker.Infrastructure/"]
RUN dotnet restore "CodeSolvedTracker.API/CodeSolvedTracker.API.csproj"

COPY . .
WORKDIR "/src/CodeSolvedTracker.API"
RUN dotnet build "CodeSolvedTracker.API.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "CodeSolvedTracker.API.dll"]
