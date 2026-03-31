FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/EcommerceAPI.API/EcommerceAPI.API.csproj", "src/EcommerceAPI.API/"]
COPY ["src/EcommerceAPI.Application/EcommerceAPI.Application.csproj", "src/EcommerceAPI.Application/"]
COPY ["src/EcommerceAPI.Infrastructure/EcommerceAPI.Infrastructure.csproj", "src/EcommerceAPI.Infrastructure/"]
COPY ["src/EcommerceAPI.Domain/EcommerceAPI.Domain.csproj", "src/EcommerceAPI.Domain/"]

RUN dotnet restore "src/EcommerceAPI.API/EcommerceAPI.API.csproj"

COPY . .

RUN dotnet publish "src/EcommerceAPI.API/EcommerceAPI.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EcommerceAPI.API.dll"]
