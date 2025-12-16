# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ParkingReg.Web/*.csproj ./ParkingReg.Web/
RUN dotnet restore ParkingReg.Web/*.csproj

# Copy everything else and build
COPY ParkingReg.Web/. ./ParkingReg.Web/
WORKDIR /app/ParkingReg.Web
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set environment variables for the database
ENV DATABASE_IP=127.0.0.1
ENV DATABASE_PORT=3306
ENV DATABASE_PASSWORD=mysecretpassword

# Expose the port the app runs on
EXPOSE 80
EXPOSE 443

# Set the entry point
ENTRYPOINT ["dotnet", "ParkingReg.Web.dll"]