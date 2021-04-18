FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .
COPY Pokedex.Core/*.csproj Pokedex.Core/
COPY Pokedex.Core.UnitTests/*.csproj Pokedex.Core.UnitTests/
COPY Pokedex.WebApi/*.csproj Pokedex.WebApi/
COPY Pokedex.WebApi.IntegrationTests/*.csproj Pokedex.WebApi.IntegrationTests/
RUN dotnet restore

COPY . .
WORKDIR /src
RUN dotnet build -c Release -o /app/build
RUN dotnet test -c Release 

FROM build AS publish
WORKDIR /src/Pokedex.WebApi
RUN dotnet publish "Pokedex.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokedex.WebApi.dll"]
