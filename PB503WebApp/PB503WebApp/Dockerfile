# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Layihə faylını kopyalayın və restore əmri verin
COPY PB503WebApp/PB503WebApp.csproj ./PB503WebApp/
RUN dotnet restore PB503WebApp/PB503WebApp.csproj

# Layihə kodlarını kopyalayın
COPY . .

# Layihəni "Publish" etmək üçün çalışma mühitini təyin edin
WORKDIR /app/PB503WebApp
RUN dotnet publish -c Release -o /out

# Run image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Əvvəlki addımda yaradılan faylları kopyalayın
COPY --from=build /out .

# Portu açın
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Tətbiqin işə salınması
ENTRYPOINT ["dotnet", "PB503WebApp.dll"]
