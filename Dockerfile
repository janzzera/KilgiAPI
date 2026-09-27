# Step 1: Use the official .NET SDK image to build the app
FROM ://microsoft.com AS build-env
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Step 2: Build the runtime image
FROM ://microsoft.com
WORKDIR /app
COPY --from=build-env /app/out .

# Render dynamically assigns a port via the PORT environment variable. 
ENV ASPNETCORE_URLS=http://+:${PORT}

ENTRYPOINT ["dotnet", "KilgiAPI.dll"]