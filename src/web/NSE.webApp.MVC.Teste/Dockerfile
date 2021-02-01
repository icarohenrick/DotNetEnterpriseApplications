#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/web/NSE.webApp.MVC.Teste/NSE.webApp.MVC.Teste.csproj", "src/web/NSE.webApp.MVC.Teste/"]
RUN dotnet restore "src/web/NSE.webApp.MVC.Teste/NSE.webApp.MVC.Teste.csproj"
COPY . .
WORKDIR "/src/src/web/NSE.webApp.MVC.Teste"
RUN dotnet build "NSE.webApp.MVC.Teste.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.webApp.MVC.Teste.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.webApp.MVC.Teste.dll"]