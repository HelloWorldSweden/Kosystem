#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Kosystem/Kosystem.csproj", "Kosystem/"]
COPY ["Kosystem.Shared/Kosystem.Shared.csproj", "Kosystem.Shared/"]
COPY ["Kosystem.Utility/Kosystem.Utility.csproj", "Kosystem.Utility/"]
RUN dotnet restore "Kosystem/Kosystem.csproj"
COPY . .
WORKDIR "/src/Kosystem"
RUN dotnet build "Kosystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kosystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kosystem.dll"]