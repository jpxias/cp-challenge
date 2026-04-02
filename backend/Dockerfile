# --- Stage 1: Build ---
    FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
    WORKDIR /src
    
    # 1. Copy only the main project file
    # We don't copy the .slnx file to avoid building tests in Docker
    COPY CivicPlusChallenge/*.csproj ./CivicPlusChallenge/
    RUN dotnet restore CivicPlusChallenge/CivicPlusChallenge.csproj
    
    # 2. Copy the source code for the main project
    COPY CivicPlusChallenge/ ./CivicPlusChallenge/
    
    # 3. Publish ONLY the specific project, NOT the whole solution
    WORKDIR /src/CivicPlusChallenge
    RUN dotnet publish CivicPlusChallenge.csproj -c Release -o /app/publish
    
    # --- Stage 2: Runtime ---
    FROM mcr.microsoft.com/dotnet/aspnet:10.0
    WORKDIR /app
    COPY --from=build /app/publish .
    ENTRYPOINT ["dotnet", "CivicPlusChallenge.dll"]