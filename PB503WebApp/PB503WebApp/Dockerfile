# Dockerfile içində `WORKDIR` və `COPY` əmrini düzəldək.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Layihə faylını doğru yolda kopyalayırıq
COPY PB503WebApp/PB503WebApp.csproj ./PB503WebApp/
RUN dotnet restore PB503WebApp/PB503WebApp.csproj

COPY . .
WORKDIR /app/PB503WebApp
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /out .

# URL konfiqurasiyasını və portu təyin edirik
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "PB503WebApp.dll"]
