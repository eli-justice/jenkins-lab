# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /app
COPY . .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o out

# final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app/out .
ENV ASPNETCORE_ENVIRONMENT=Eganow
EXPOSE 80 443
CMD ["dotnet", "Egapay.Customer.Business.Gateway.API.dll"]
