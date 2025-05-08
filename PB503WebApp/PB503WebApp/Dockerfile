# 1. Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Layihə faylını əlavə et və restore et
COPY PB503WebApp.csproj ./
RUN dotnet restore

# Qalan faylları da əlavə et və build et
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "PB503WebApp.dll"]
