# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src

# copy solution file (located inside src folder in repo)
COPY src/Egapay.Customer.Business.Gateway.sln ./src/

# copy all projects into /src/src/
COPY src/Egapay.Customer.Business.Gateway.API ./src/Egapay.Customer.Business.Gateway.API
COPY src/Egapay.Customer.Business.Gateway.Common ./src/Egapay.Customer.Business.Gateway.Common
COPY src/Egapay.Customer.Business.Gateway.MerchantOnboarding ./src/Egapay.Customer.Business.Gateway.MerchantOnboarding
COPY src/Egapay.Customer.Business.Gateway.MerchantTransactions ./src/Egapay.Customer.Business.Gateway.MerchantTransactions

# restore using the solution file
WORKDIR /src/src
RUN dotnet restore

# build + publish API project
RUN dotnet publish Egapay.Customer.Business.Gateway.API/Egapay.Customer.Business.Gateway.API.csproj \
    -c Release -o /app/out

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app/out .
ENTRYPOINT ["dotnet", "Egapay.Customer.Business.Gateway.API.dll"]
