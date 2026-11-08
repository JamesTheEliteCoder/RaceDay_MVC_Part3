
# Stage 1: Build the MVC application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the MVC project file into the container
COPY ["RaceDay_MVC_Part3.csproj", "./"]

# Restore the project's NuGet dependencies
RUN dotnet restore

# Copy the remaining application files into the container
COPY . .

# Publish the MVC application in Release configuration
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# Set the working directory
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Configure the application to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Document the port used by the application
EXPOSE 8080

# Start the MVC application
ENTRYPOINT ["dotnet", "RaceDay_MVC_Part3.dll"]