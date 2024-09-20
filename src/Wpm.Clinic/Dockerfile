#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the official .NET 8 SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything from the current directory to the working directory in the container
COPY . .

# Restore dependencies (use .csproj to minimize unnecessary restores)
RUN dotnet restore

# Build the project
RUN dotnet publish -c Release -o out

# Use a runtime image to run the published app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/out .

EXPOSE 8080

# Set the entry point for the container to run the app
# ENTRYPOINT ["dotnet", "TestApiDocker.dll"]
CMD ["dotnet", "Wpm.Clinic.dll"]