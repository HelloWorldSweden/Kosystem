FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Kosystem.Web/Kosystem.Web.csproj", "Kosystem.Web/"]
RUN dotnet restore "Kosystem.Web/Kosystem.Web.csproj"
COPY . .
WORKDIR "/src/Kosystem.Web"
RUN dotnet build "Kosystem.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Kosystem.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Kosystem.Web.dll"]