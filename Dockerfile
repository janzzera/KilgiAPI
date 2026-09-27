# Step 1: Build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy project files first to leverage Docker layer caching
COPY *.csproj ./
RUN dotnet restore

# Copy all source files and publish release artifacts
COPY . ./
RUN dotnet publish -c Release -o out

# Step 2: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy build artifacts from builder stage
COPY --from=build-env /app/out .

# Render injects the PORT variable at runtime.
# Using a shell wrapper or setting ASPNETCORE_HTTP_PORTS handles Render's dynamic port assignment.
ENV ASPNETCORE_HTTP_PORTS=${PORT:-8080}

ENTRYPOINT ["dotnet", "KilgiAPI.dll"]