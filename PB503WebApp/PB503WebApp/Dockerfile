# 1. Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# PB503WebApp.csproj faylını kopyalayın
COPY PB503WebApp/PB503WebApp.csproj ./PB503WebApp/
RUN dotnet restore ./PB503WebApp/PB503WebApp.csproj

# Bütün qovluğu kopyalayın
COPY PB503WebApp/ ./PB503WebApp/
WORKDIR /app/PB503WebApp
RUN dotnet publish -c Release -o out

# 2. Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/PB503WebApp/out ./
ENTRYPOINT ["dotnet", "PB503WebApp.dll"]
