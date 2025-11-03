# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY global.json ./
COPY ShopSim.sln ./

# Copy project file
COPY ShopSim/ShopSim.csproj ShopSim/

# Restore dependencies
RUN dotnet restore ShopSim/ShopSim.csproj

# Copy source code
COPY ShopSim/ ShopSim/

# Build the application
WORKDIR /src/ShopSim
RUN dotnet build ShopSim.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish ShopSim.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install MySQL client for debugging/tools (optional)
RUN apt-get update && apt-get install -y default-mysql-client && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Entry point
ENTRYPOINT ["dotnet", "ShopSim.dll"]
