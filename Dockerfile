# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the csproj file first
COPY Egapay.Customer.Business.Gateway.API/*.csproj ./Egapay.Customer.Business.Gateway.API/
RUN dotnet restore ./Egapay.Customer.Business.Gateway.API/Egapay.Customer.Business.Gateway.API.csproj

# Copy the rest of the code
COPY . .
WORKDIR /src/Egapay.Customer.Business.Gateway.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Egapay.Customer.Business.Gateway.API.dll"]
