#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CarbonKitchen.Ingredients.Api/CarbonKitchen.Ingredients.Api.csproj", "CarbonKitchen.Ingredients.Api/"]
RUN dotnet restore "CarbonKitchen.Ingredients.Api/CarbonKitchen.Ingredients.Api.csproj"
COPY . .
WORKDIR "/src/CarbonKitchen.Ingredients.Api"
RUN dotnet build "CarbonKitchen.Ingredients.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarbonKitchen.Ingredients.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarbonKitchen.Ingredients.Api.dll"]