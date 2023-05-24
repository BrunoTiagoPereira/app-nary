#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AppNary.Host/AppNary.Host.csproj", "src/AppNary.Host/"]
RUN dotnet restore "src/AppNary.Host/AppNary.Host.csproj"
COPY . .
WORKDIR "/src/src/AppNary.Host"
RUN dotnet build "AppNary.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppNary.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppNary.Host.dll"]