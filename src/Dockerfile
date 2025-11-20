# build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src

# copy solution file
COPY Egapay.Customer.Business.Gateway.sln ./

# copy all project files
COPY Egapay.Customer.Business.Gateway.API ./Egapay.Customer.Business.Gateway.API
COPY Egapay.Customer.Business.Gateway.Common ./Egapay.Customer.Business.Gateway.Common
COPY Egapay.Customer.Business.Gateway.MerchantOnboarding ./Egapay.Customer.Business.Gateway.MerchantOnboarding
COPY Egapay.Customer.Business.Gateway.MerchantTransactions ./Egapay.Customer.Business.Gateway.MerchantTransactions

# restore dependencies
RUN dotnet restore

# build + publish API project
RUN dotnet publish ./Egapay.Customer.Business.Gateway.API/Egapay.Customer.Business.Gateway.API.csproj \
    -c Release -o /app/out

# final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app/out .
ENTRYPOINT ["dotnet", "Egapay.Customer.Business.Gateway.API.dll"]
