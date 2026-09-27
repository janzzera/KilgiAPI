# Step 1: Build environment
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /src

# Copy only the .csproj first to preserve Docker layer caching
COPY KilgiAPI/*.csproj ./KilgiAPI/
RUN dotnet restore KilgiAPI/*.csproj

# Copy the rest of the project source code and publish
COPY KilgiAPI/ ./KilgiAPI/
WORKDIR /src/KilgiAPI
RUN dotnet publish -c Release -o /app/out

# Step 2: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/out .

# Render dynamically sets PORT; ASPNETCORE_HTTP_PORTS reads it in .NET 8
ENV ASPNETCORE_HTTP_PORTS=${PORT:-8080}

ENTRYPOINT ["dotnet", "KilgiAPI.dll"]