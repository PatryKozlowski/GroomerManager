# Użyj obrazu SDK do budowy projektu
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Kopiuj pliki projektu i przywróć zależności
COPY ["GroomerManager.API/GroomerManager.API.csproj", "GroomerManager.API/"]
COPY ["GroomerManager.Application/GroomerManager.Application.csproj", "GroomerManager.Application/"]
COPY ["GroomerManager.Domain/GroomerManager.Domain.csproj", "GroomerManager.Domain/"]
COPY ["GroomerManager.Infrastructure/GroomerManager.Infrastructure.csproj", "GroomerManager.Infrastructure/"]
COPY ["GroomerManager.sln", "./"]
RUN dotnet restore "GroomerManager.API/GroomerManager.API.csproj"

# Skopiuj cały kod źródłowy i zbuduj aplikację
COPY . .
WORKDIR "/src/GroomerManager.API"
RUN dotnet build "GroomerManager.API.csproj" -c Release -o /app/build

# Publikuj aplikację
FROM build AS publish
RUN dotnet publish "GroomerManager.API.csproj" -c Release -o /app/publish

# Finalny obraz na podstawie ASP.NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GroomerManager.API.dll"]
