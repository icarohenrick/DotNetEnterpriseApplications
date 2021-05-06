#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./services/NSE.Cliente.API/NSE.Clientes.API.csproj", "services/NSE.Cliente.API/"]
COPY ["./building blocks/NSE.Core/NSE.Core.csproj", "building blocks/NSE.WebApp.Core/"]
COPY ["./building blocks/NSE.WepAPI.Core/NSE.WepAPI.Core.csproj", "building blocks/NSE.WebApi.Core/"]
COPY ["./building blocks/NSE.MessageBus/NSE.MessageBus.csproj", "building blocks/NSE.MessageBus/"]

RUN dotnet restore "services/NSE.Cliente.API/NSE.Clientes.API.csproj"
COPY . .
WORKDIR "services/NSE.Cliente.API"
RUN dotnet build "NSE.Clientes.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.Clientes.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.Clientes.API.dll"]