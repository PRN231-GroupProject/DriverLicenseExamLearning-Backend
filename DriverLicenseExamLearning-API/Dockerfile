# Use the .NET 7 as the base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./DriverLicenseExamExchange-API.csproj", "DriverLicenseExamExchange-API/"]
RUN dotnet restore "./DriverLicenseExamExchange-API.csproj"
COPY . .
WORKDIR "/src/DriverLicenseExamExchange-API"
RUN dotnet build "DriverLicenseExamExchange-API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "DriverLicenseExamExchange-API.csproj" -c Release -o /app/publish

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DriverLicenseExamExchange-API.dll"]

