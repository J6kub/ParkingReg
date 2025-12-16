# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set environment variables for the database
ENV DATABASE_IP=mariadb
ENV DATABASE_PORT=3306
ENV DATABASE_PASSWORD=mysecretpassword
ENV ASPNETCORE_URLS=http://+:5050

# Expose HTTP port
EXPOSE 5050

# Entry point
ENTRYPOINT ["dotnet", "ParkingReg.Web.dll"]
