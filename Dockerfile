# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy and restore the DAL project
COPY DAL/*.csproj DAL/
RUN dotnet restore DAL/DAL.csproj

# Copy and restore the BLL project
COPY BLL/*.csproj BLL/
RUN dotnet restore BLL/BLL.csproj

# Copy and restore the API project
COPY API/*.csproj API/
RUN dotnet restore API/API.csproj

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet build API/API.csproj -c Release -o /app/build

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish API/API.csproj -c Release -o /app/publish

# Stage 3: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=publish /app/publish .

# Expose the port for the API
EXPOSE 80

# Set the entry point for the API
ENTRYPOINT ["dotnet", "API.dll"]
