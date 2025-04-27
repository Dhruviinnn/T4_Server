# Use official .NET SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the project files
COPY *.csproj ./
RUN dotnet restore

# Copy everything and build the project
COPY . ./
RUN dotnet publish -c Release -o out

# Use a smaller runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the API port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "TimeForthe.dll"]
